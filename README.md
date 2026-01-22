# Docker-Mudblazor

## About Docker-Mudblazor

### About Synology-Docker

Around 2014 when I was working at Vanderlande in Veghel, I had many collegues that were busy with home automation.
Some where showing live views of their home camara on their phones.
So you could see some pet walking around or so (and let's be silent about their partners, haha)
Most of them were at least having some kind of NAS (Network Attached Storage) in their home and most of them were really enthousiastic about Synology.
At that time, I was hardly having time for solo-hobby projects at home, and besides storing data, I could not really find a use case for having such a NAS.
I mostly store my data on external harddrives, so no real need for a NAS.

But some years later, in May 2018, I could not stop myself, so I bought a Synology DS218+.
I started to use it as a file share, so on our laptops, it was mounted as a shared drive.
And I also started to use their VPN stuff, so I could make my mobile phone connect to the Synology VPN.
However, the VPN stuff was not really working well, so I stopped doing this.
After mainly using it for storing files, thunder struck at home one year later and it destroyed some things in my home, including the Synology.
The harddrive, insite, some good-old (yet slow) Western Digital Red 3TB was still intact.
However, I did not take time to reorder one...
Until at the last day of 2020, when I had some time left... I ordered the DS220+ (as the older DS218+ was less available and more expensive) together with a UPS (to protect the Synology from future thunder strikes).
However, after ordering it, I didn't find the time to re-install all that until the end of 2025.

Luckily after four years in a box, the hardware still works fine.
Also the disk, which was 'stored' in the old broxen DS218+ was still fine.
So after reinstalling DSM, started using the Synology as a NAS with file mounts from our local computers pointing to it...
...and even after trying Synology Photo's, which is quite funny
...it was time to try something new.

![Picture of a network switch, a Synology DS220+ and a UPS.](/assets/images/SynologyDS220PlusWithUPS.jpeg)

I saw a .NET Zuid presentation about Mud Blazor, while I never played with Blazor before, and I decided that I am going to try to get some Mud Blazor application to run via Docker on my Synology.
The cool thing is, that this Synology only uses in between 4.41W (Idle) and 14.69W (Max) of power, so there is no real 'harm' in having this thing run all the time.
Especially not when comparing it to my Desktop, that needed a 500W Power Supply as the 400W Power Supply could not handle the start-up of the graphical card.
Just to get an idea about about which CPU all this is running...

| Machine  | CPU | Speed | Number of Cores | CPU Mark | Comment |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
| Synology DS220+  | Intel Celeron J4025  | 2Ghz  | 2  | 1452  | Slightly better than the CPU that was in the DS218+  |
| My 2016 laptop  | Intel Core i5-6200U | 2.3Ghz  | 2  | 2988  | A laptop that is 10 years old and not officially capable of running Windows 11  |
| My 2017 desktop  | AMD Ryzen 5 3600 | 3.6Ghz  | 6  | 17681  | It originally contained another CPU, but this was the cheapest replacement that officially is capable of running Windows 11  |
| My IPhone 13 mini | Apple A15 Bionic | 3.23Ghz | 6 | 9890 | The smallest OK performing phone that I bought in 2023 which came to market in 2021 |

The CPU Mark (higher is better) is an indication of how fast a CPU is, which is benchmarked by [PassMark Software](https://www.cpubenchmark.net/cpu_list.php)

## Getting simple Docker-Mudblazor to work
### About Docker-Mudblazor

So after seeing the presentation, I was wondering on what to make in MudBlazor.
Well initially, it was just time to find out if the default Hello-MudBlazor would run.
I was even in doubt over in which editor to play around.
As there is no point in hiding my source, Visual Studio by [Microsoft](https://visualstudio.microsoft.com/free-developer-offers/) or Rider [by JetBrains](https://www.jetbrains.com/rider/buy/?section=personal&billing=yearly) would be free anyway.
But for some reason, I fell like experimenting with Visual Studio Code, which is free by definition and which I have only used for some web development in the past.

So initially it was just a matter of following the [MudBlazor Tutorial](https://mudblazor.com/getting-started/installation) in order to get things to work locally in Windows.
Off course that part is easy. So the next step is running all that in Docker, starting on my local PC with [Docker Desktop](https://docs.docker.com/desktop/).
As the example solution of that MudBlazor tutorial, holds two projects, its a bit tricky. It's main project, named MyApplication is the stuff that runs and makes use of the second project, MyApplication.Client.
So both projects need to be build, which makes the compose file and the docker command a bit more tricky as all files need to be gathered.
For the compose file, the best docker images that I could currently select from the [Docker Hub](https://hub.docker.com/) were the dotnet asp.net 10 image (runtime) and the dotnet sdk 10 image for doing the build.
I came to the following compose file, which I put in the main project (MyApplication) directory:
```
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MyApplication/MyApplication.csproj", "MyApplication/" ]
COPY ["MyApplication.Client/MyApplication.Client.csproj", "MyApplication.Client/" ]

RUN dotnet restore "MyApplication/MyApplication.csproj"
COPY . .
WORKDIR "/src/MyApplication"
RUN dotnet publish "MyApplication.csproj" -c release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "MyApplication.dll" ]
```

And than from the Power Shell, I needed to go one level higher up (Solution-level AKA git-root folder) and execute the following command:
```
docker build -t hello-docker-mudblazor -f MyApplication\Dockerfile .
```

After being patient, you can find the image in Docker Desktop:

![Docker Desktop showing the new image.](/assets/images/NewImageInDockerDesktop.png)

Then when clicking the play button in Docker Desktop, you can name it and define the port mappings. I mapped 5580 to 8080 to access it.

![Starting a new container from Docker Desktop.](/assets/images/RunImageInDockerDesktopAsNewContainer.png)

After that, you will jump to the log file of the newly running container, but also from the overview, you can see it.

![Seeing the running container in Docker Desktop.](/assets/images/SeeNewRunningContainerInDockerDesktop.png)

And finally when clicking on the port mapping (5580-8080) you will jump in a new browser via the localhost on the mapped port and you will access the app.

![Seeing the running container in Docker Desktop.](/assets/images/AccessNewContainerViaDefinedPort.png)

Nice to see, that this Hello-World thingy is running on the local Docker.
Next step is getting it to run on The Synology...

### About Docker-Mudblazor-Synology

After playing around with the Synology, which runs on DSM 7.3.2 it was time to get Docker Stuff running on it.
Via Package Center, You can find it, named as Container Manager, which is a bit like the Docker Desktop for Synology.
Via the GUI, you find public container images that are stored on the [Docker Hub](https://hub.docker.com/), which is actually the same 'shared library' on which we found the base images for creating our own docker image, via the Register tab.
Just to validate that my Synology Docker stuff was working well, I experimented with installing Uptime Kuma (some easy monitoring tool) on it.
I was helped by [this video](https://www.youtube.com/watch?v=X0qGNgmCIGw) that explains the working of Container Manager
From the Registry Tab, you can select a public image, click download and then select a version (like latest) and download the image.
Than when the image is downloaded, you can start it, give it a name and assign a port.
So after seeing that running, it's time to get the Docker-MudBlazor stuff to run on the Synology.
After installing the [Container Tools extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-containers) to Visual Studio Code, you can find the created images in the Containers tab.
And from there, you can push these images to the [Docker Hub](https://hub.docker.com/).

![Push Image To Docker Hub](/assets/images/PushImageToDockerHub.png)

See this URL for [the list of my example images](https://hub.docker.com/repository/docker/ivoraedts/hello-docker-mudblazor/tags) by tag.

So from the Registry Tab, select the image, click download and then select a version (like latest) and download the image. As latest was later overwritten, you can still get this first version by tag 0.1.

![Select image to download](/assets/images/Synology-SelectImageToDownload.png)

Than from the Image Tab, select it, click on Run (haha in Dutch that is Uitvoeren), give it a name and click on Next (which is Volgende in Dutch)..

![Run image as new container in Synology](/assets/images/Synology-RunImageInAsNewContainer.png)

Then assign some port-mapping so you can access it from the outside. (and click complete in the next window)

![Run image as new container in Synology Step 2](/assets/images/Synology-RunImageInAsNewContainerStep2.png)

Then you can see the newly created container on the Container tab and access it via the define port on the IP-address of the Synology or via the Synology Quick Connect.

![See running containers](/assets/images/Synology-SeeRunningContainer.png)

![See app running in browser](/assets/images/Synology-AccessNewContainerViaDefinedPort.png)

Nice to see, that this Hello-World thingy is running on the local Synology now.
Next step is some more realistic project that includes some database reading and writing...

## Getting some more realistic Docker-Mudblazor version to work
### Adding a Database some Database GUI to Synology-Docker
So, as most projects contain databases, it's time to add a database to it and some tooling to see it.
Alternatively I could have picked adding reverse proxy tooling, message queuing, S3 file storage, caching, etc.
A database was just the best example and the most practical one to do some nice playing around once installed.

I decided to go for PostgreSQL as this is one of the popular open-source relational databases.
Next to this, I decided to initially go for pgAdmin 4 as the GUI for doing the database administrations and validations.
I wanted to start with local development. But with the next step in mind: running the stuff in Docker, I decided it is probably more effective if I directly run pgAdmin and PostgreSQL in Docker and then connect my local Docker-MudBlazor to those over some exposed port.
So this means, I will have 3 running projects / images:
1. That MudBlazor Project
2. PostgreSQL (starting with the latest version of the [official image](https://hub.docker.com/_/postgres) )
3. PG Admin (starting with the latest version of the [most pulled image](https://hub.docker.com/r/dpage/pgadmin4) )

It will be trivial to use Docker Compose to get these containers to start and communicate to each other.
So it makes sense to define a Docker Network, so the containers can access each other via the servicename.
Also for both of them, I defined a volume mapping, so the data is stored on disk.
So when the containers and images are destroyed and replaced, the data will still be there.
I added some simple username plus password for both postrgres and pgadmin.
As I have no postgres or so running on the localhost, I have exposed the default port (5432).
For pgadmin (which runs on 80, I defined the host port as 5050).
So just for getting the database and administration to run, it resulted in this docker-compose file, that I have named ``docker-compose-db.yml``:

```
networks:
  docker-mudblazor:
    external: false

services:
  postgres:
    image: postgres:latest
    container_name: docker-mudblazor.postgres
    volumes:  
      - ./docker/pgdata:/var/lib/postgresql
    networks:
      - docker-mudblazor
    environment:
      POSTGRES_USER: user-name
      POSTGRES_PASSWORD: strong-password
    ports:
      - "5432:5432"     

  pgadmin:
    image: dpage/pgadmin4:latest
    container_name: docker-mudblazor.pgadmin
    volumes:
      - ./docker/pgadmin:/var/lib/pgadmin
    environment:
      PGADMIN_DEFAULT_EMAIL: user-name@domain-name.com
      PGADMIN_DEFAULT_PASSWORD: strong-password
    networks:
      - docker-mudblazor
    ports:
      - "5050:80"
```

So when running ``docker compose -f docker-compose-local.yml up`` , the images are downloaded and pushed into containers within the same network. (That filename only needs to be specified if the docker compose file to use differs from the default).
When opening PG Admin via the ``http://localhost:5050/``, you must use the login and password from the dockerfile. (so ``user-name@domain-name.com`` as login and ``strong-password`` as password).

![Sign in to pgadmin](/assets/images/SignInToPgAdmin.png)

Then via Servers, add a new Registry to the postgres database. Then you can connect via the servicename ``postgres`` over port ``5432`` via username ``user-name`` and ``strong-password`` as password.

![connect to postgres](/assets/images/ConnectToPostgres.png)

If this all works, you can create databases and if desired some tables in it.

After making a Database, it makes sense to validate your volume mapping by browsing to the folders defined for postgres (./docker/pgdata) and pgadmin (./docker/pgadmin).
Postgres should have made sub-folders: 18 and then docker and then a lot of other folders in it:

![Local folder contents of postgres volume](/assets/images/PostgresMount.png)

PgAdmin should just create some folders and files in your given location like this:

![Local folder contents of pgadmin volume](/assets/images/PgAdminMount.png)

So then the next step is connecting to the postgres database from the MudBlazor application....

If you cannot see this data, something went wrong with the mapping.
This means, once you delete the container and image, and then recreate an image and start a container, you will notice all data was deleted.
So then you will lose all databases that were created in postgres. And all stored connections that were used in pgadmin.

### Accessing the Database from the Local MudBlazor application

Off course it's nice to arrange the data access via Entity Framework Core, so I added references to ``Microsoft.EntityFrameworkCore``, ``Microsoft.EntityFrameworkCore.Tools`` and ``Npgsql.EntityFrameworkCore.PostgreSQL``.
In order to be able to access the database from the server-part of the application, some trivial things need to be added to the MyApplication (Server-part) project.
I started with designing the first table, calendar needs to look like by putting the data structure into a class, named ``CalendarEvent``:

```
namespace MyApplication.Data
{
    public class CalendarEvent
    {
        public int Id {get; set;}
        public required string Title {get; set;}
        public required string Description {get; set;}
    }
}
```

Then it's time to add a Database context and add a DbSet of that type. I named it ``MudBlazorDbContext`` :
```
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Data
{
    public class MudBlazorDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MudBlazorDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //connect to postgress with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("MudBlazorDatabase"));
        }

        public DbSet<CalendarEvent> CalendarEvents {get; set;}
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMudBlazorDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MudBlazorDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("MudBlazorDatabase"),
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
                )
            );

            return services;
        }
    }
}
```
To wire it up, in the ``Program.cs``, I added:
```
builder.Services.AddMudBlazorDbContext(builder.Configuration);
```

before ``var app = builder.Build();`` and after that I added:
```
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyApplication.Data.MudBlazorDbContext>();
    context.Database.Migrate();
}
```

note that the ``context.Database.Migrate();`` part ensures that the migrations will be executed on start-up of the application. (assuming it can connect correctly to the database)

Then in ``appsettings.json`` // ``appsettings.Development.json`` I need to add the connection string that I want to use for making a connection from my local-running Application.
In order to connect from my local-running application to the postgres database running on my local docker, I must use port 5432 that I exposed, so it looks like:

```
    "ConnectionStrings": {
    "MudBlazorDatabase": "Host=localhost:5432; Database=SomeTestDatabase; Username=user-name; Password=strong-password"
  }
```
Keep in mind, that in the later stage, when MudBlazor also runs in docker, we can directly connect to ``Host=postgres:5432;`` , like how we connected from PgAdmin to PostgresQL before.

And than from the Power Shell, I needed to first install the Entitiy Framework tooling:
```
dotnet tool install --global dotnet-ef
```
and then add that migration:
```
dotnet ef migrations add InitialDatbaseMigration
```

Than if all went well, running the application should result in running the migrations.
Probably you should first arrange that the database exist (which you can do via pgadmin).
And than the application will connect to that database and perform the migrations by creating the intitial table and the migrations table:

![Database as seen from PgAdmin](/assets/images/SeeingTheDatabaseFromPgAdmin.png)

Note that before running this, I created the database with that blabla table. Running the migrations will not touch that existing table.
Also note that you can repeat running the migration by stoppping the application, deleting the CalendarEvents table and the _EFMigrationsHistory table and than again starting the application.

This is usefull for the next step: accessing the database from the application when that application runs in Docker...

### Accessing the Database from the MudBlazor application running in Docker

While the previous version of the MudBlazor application was created in Docker via manually running Docker build, it's now time to get this to work via Docker Compose.
Earlier we made a Docker Compose file for getting postgres and pgadmin to run. Now it's time to add the MudBlazor application to that, so all three of them end-up in one Docker Compose file.
I named this version ``docker-compose-local.yml`` as this is the file that will locally build that docker image, which will run as the docker-mudblazor service.
I also added some dependancy-logic in the compose file, so the docker-mudblazor will start after postgres is running. (otherwise it will crash out)
So I kind of ended up with the following compose file:

```
networks:
  docker-mudblazor:
    external: false

services:
  docker-mudblazor:
    build:
      context: .
      dockerfile: MyApplication/Dockerfile
    depends_on:
      postgres: 
        condition: service_healthy
        restart: true
    networks:
      - docker-mudblazor
    volumes:
      - ./docker/mudblazor:/app/data
    ports:
      - "3080:8080"
      - "3081:8081"
    environment:
      ConnectionStrings__MudBlazorDatabase: "Host=postgres:5432; Database=SomeTestDatabase; Username=user-name; Password=strong-password"
      ASPNETCORE_ENVIRONMENT: Production

  postgres:
    image: postgres:latest
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -d SomeTestDatabase"]
      interval: 10s
      retries: 5
      start_period: 10s
      timeout: 10s
    container_name: docker-mudblazor.postgres
    volumes:  
      - ./docker/pgdata:/var/lib/postgresql
    networks:
      - docker-mudblazor
    environment:
      POSTGRES_USER: user-name
      POSTGRES_PASSWORD: strong-password
    ports:
      - "5432:5432"     

  pgadmin:
    image: dpage/pgadmin4:latest
    container_name: docker-mudblazor.pgadmin
    volumes:
      - ./docker/pgadmin:/var/lib/pgadmin
    environment:
      PGADMIN_DEFAULT_EMAIL: user-name@domain-name.com
      PGADMIN_DEFAULT_PASSWORD: strong-password
    networks:
      - docker-mudblazor
    ports:
      - "5050:80"
```

Then via Power Shell you can run the application as follows:
```
docker compose -f docker-compose-local.yml up
```
As described before, the database connection can be tested by deleting the CalendarEvents table and the _EFMigrationsHistory table and than (re)again starting the application and than validating those tables are recreated.
On the other hand... if the application itself does not connect, it will crash out anyway.
So if it runs and you can access it via ``http://localhost:3080/`` it should be fine.
After seeing that this works from on the local PC via Docker Desktop, it's interesting to see if this will also work on the Synology...

### Accessing the Database from the MudBlazor application running in Container Manager on the Synology

So the fist thing to do after those migrations were added to the code, is similar to how we earlier published the application image to the [Docker Hub](https://hub.docker.com/) via the Containers tab of Visual Studio Code.
Note that we only need to push the docker-mudblazor image, as the postgres and pgadmin images are already present in the Docker Hub.
We got the docker-mudblazor on the Synology on the Container Manager via its Register-tab and Image-tab.
Now we will use the Project-tab to get this to work. And again I was helped a bit by [this video](https://www.youtube.com/watch?v=X0qGNgmCIGw) that explains the working of Container Manager.

From the Project Tab, you can create a new project by clicking on Create (or Maken in Dutch 😁).
Then you give the project a name, like ``hello-docker-mudblazor``, you can define the root-path / volume of the project, like ``/volume1/docker/hello-docker-mudblazor``, and can choose to create a new docker-compose.yml.
This is similar to the docker-compose files that we were using for getting things to work on Docker Desktop.
Only now the volumes will be relative to the project-volume.
So when I mount postgres on ``./docker/pgdata`` I can find the data on ``/volume1/docker/hello-docker-mudblazor/docker/pgdata``.
This is nice, so if you run more projects, the volumes are not mixed in the same folders.

While getting things to run, I had problems with running pgadmin running on the Synology. 😠
For some reason, it lacked permissions to write on the given volume, while postgres did not have this problems.
I saw some work-around of giving ownership of the folder to the ContainerManager-group, but I didn't like that.
Especially when that is not needed for postgres itself.
So I came up with another work around: Ditch PgAdmin on the Synology and use adminer instead.
Adminer is not so nice as PgAdmin, but it does not need a volume as it stores all connections in the browser cache.
So I ended up with the following docker-compose project yml that I have stored in git as ``synology-docker-compose-yml``:

```
networks:
  docker-mudblazor:
    external: false

services:
  docker-mudblazor:
    image: ivoraedts/hello-docker-mudblazor:latest
    depends_on:
      postgres: 
        condition: service_healthy
        restart: true
    networks:
      - docker-mudblazor
    volumes:
      - ./docker/mudblazor:/app/data
    ports:
      - "3080:8080"
      - "3081:8081"
    environment:
      ConnectionStrings__MudBlazorDatabase: "Host=postgres:5432; Database=SomeTestDatabase; Username=user-name; Password=strong-password"
      ASPNETCORE_ENVIRONMENT: Production

  postgres:
    image: postgres:latest
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -d SomeTestDatabase"]
      interval: 10s
      retries: 5
      start_period: 10s
      timeout: 10s
    container_name: docker-mudblazor.postgres
    volumes:  
      - ./docker/pgdata:/var/lib/postgresql
    networks:
      - docker-mudblazor
    environment:
      POSTGRES_USER: user-name
      POSTGRES_PASSWORD: strong-password
    ports:
      - "1234:5432"   

  adminer:
    image: adminer:latest
    container_name: docker-mudblazor.adminer    
    networks:
      - docker-mudblazor
    ports:
      - "8080:8080"
```

As I am not sure if I will use postgres from the outside, I decided to map the port to 1234. But maybe I could remove that mapping as well and prevent access from the outside.
Than for Adminer, I just mapped the 8080 to 8080, so over that port I can access adminer.
The Docker-Mudblazor app itself, will again be acccesible on 3080.
Ah..and because of that, it makes sense that before starting the project, the previous container must be stopped. (otherwise that occupies port 3080)
When all is configured fine, starting up the project will result in the images being downloaded and the containers will be started and keep running.
If something goes wrong (like what I had with pgadmin), the container will stop and still be there and it will have logging that hopefully shows what goes wrong.

![See running containers](/assets/images/ContainersRunningInSynology.png)

![See adminer running in browser](/assets/images/AdminerRunning.png)

![See table via adminer running in browser](/assets/images/SeeTableInAdminer.png)

And just like shown earlier, you should be able to access the docker-mudblazor app on the Synology via port 3080 (so via http://192.168.2.16:3080/ in my case)

Now we can connect to a database, it's time to see that we can query and add some data via the mud-blazor application...

## Quering and Adding some data from the Mud-Blazor Application
### Arranging the Http calls and the data

Now the Docker-Synology configuration stuff seems done and it's time to really do something with the data from the MudBlazor app.
The example application rus in InteractiveAuto mode, in which in is initially working as a Blazor Server application which uses SignalR communication, while it downloads the WebAssembly on the background, after which the processing is offloaded to the client and communication to the server is only done when really needed.
So next to the existing Home, Calendar and Weather tabs, I added a calendar component and suprizingly named it Get Data in the menu as I was busy with just that in mind. 😁
I added some stuff to that Calendar page, so I can try things step by step and with or without data.
Initially I was getting the data in the pageload and refreshing it after modifications, but after some initial problems I decided not to load any data on the page load.
Than I added 4 buttons: one for showing the local url, one for loading fake data, one for loading real data from the database and one for adding data to the database.
This also give me the opportunity to play with the MudBlazor table, which was easier to use than expected.
While initially failing to get it to run, the AI agent, mostly Claude Haiku 4.5, came with some good suggestions.
I suggested I should also register a HttpClient in dependancy injection and it added it for my to the ``Program.cs`` of the ``MyAplication.Client`` project 😆.
```
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
```
Then on the 'Server' application (MyApplication), I added a ``CalendarEventController``, configured a route and added a HttpGet method that returns all current records and a HttpPost method for just adding an entry to the table.
Next to this, I added a ``FakeDataController``, which was for easier pinpointing to the error: is there a problem with the SignalR routing, than the Fake data would not work... or just to the real data access, in which case the Fake data would work.
This time, there were no real tricks needed to move from running local to running in the local Docker Desktop and then to running in Synology's Container Management.
Here is a screenshot of this MudBlazor version running on the local Docker Desktop. (only difference with others is the port number and/or URL)

![MudBlazor with data running on Docker Desktop](/assets/images/MudBlazorWithData.png)

### Commenting the stuff in GitHub

When making all this documentation, I sometimes peaked at this documentation of the [markdown stuff](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax).

## HTTPS/SSL Certificate Setup

### Local Development with HTTPS

To run the application locally with HTTPS support:

1. **Generate the development certificate** (Windows):
```powershell
dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -p "MyPassword123" --trust
```

2. **Run with HTTPS support:**
```powershell
cd MyApplication
dotnet run --launch-profile both
```

This will start the application on:
- HTTP: `http://localhost:5078`
- HTTPS: `https://localhost:7063`

### Docker with HTTPS

The Docker setup includes HTTPS support via self-signed certificate.

1. **Generate the certificate locally** (see above)

2. **Copy the certificate to the Docker directory:**
```powershell
Copy-Item "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" "docker/https/aspnetapp.pfx" -Force
```

3. **Start Docker containers:**
```powershell
docker-compose -f docker-compose-local.yml up
```

This will make the application accessible on:
- HTTP: `http://localhost:3080`
- HTTPS: `https://localhost:3443`

### Certificate Security Notes

- **The PFX file is NOT committed to source control** (`docker/https/` is in `.gitignore`)
- Each developer must generate their own certificate locally
- The certificate is self-signed, so browsers will show a security warning (this is normal for development)
- For production, use a proper SSL certificate from a trusted Certificate Authority
