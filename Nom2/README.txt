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

***Due to the concurrent design of this program, the program may throw exceptions.***


PROGRAM DESIGN
The system has two array buffers, milk_buffer and cheese_buffer to represent resources milk and cheese being produced/consumed
When accessing either buffers a mutex lock is given to the current thread to enfore mutual exclusion.

Four semaphores are used, two for each buffer, to solve the producer/consumer problem.

Both "Empty" semaphores are initialized to the max number of entries a buffer can handle to populate from producing threads.
Each Empty.WaitOne() decreases the semaphore, representing the current space in a buffer.

Both "Full" semaphores are initialized to 0, blocking until there are enough entries in the buffer to continue.

The program will produce cheese by taking the top 3 entries (Ex. If there are six entries, then Cheese_Production will take entries {4, 5, 6},
append them, and then append the thread ID {4, 5})

The program will produce a burger by taking the top two entries (Ex. If there are 4 entries, then Cheese_Consumption will take entries {3, 4}
and append the two entries together)

The program will output Cheese values and Burger values when created.