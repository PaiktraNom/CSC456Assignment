RUNNING CODE

---------Through Linux (Ubuntu 20.04)---------
1) INSTALL MONO (If needed)
1a) Install Dependencies

	$ sudo apt update
	$ sudo apt install dirmngr gnupg apt-transport-https ca-certificates software-properties-common

1b) Import repository GPG key

	$ sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

1c) Add the mono repository

	$ sudo apt-add-repository 'deb https://download.mono-project.com/repo/ubuntu stable-bionic main'

1d) Install Mono

	$ sudo apt install mono-complete

2) Go to directory where Nom2.cs is

3) Compile Code

	$ mcs Nom2.cs 

4) Execute Code

	$ mono Nom2.exe

-----Through Visual Studio-----
1) Open Nom2.sln
2) Press Start button


PROGRAM DESIGN
