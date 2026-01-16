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

### About Docker-Mudblazor

This project just started with me playing around with a MudBlazor-project that I run with Docker on my Synology.

I made add some more documentation when I feel like playing around with [markdown stuff](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax).
