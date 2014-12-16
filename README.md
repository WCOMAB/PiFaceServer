#PiFace Server

PiFace Server is a simple socket based client / server for controlling an `RaspberyPi` `PiFace` I/O board via a local network via socket or Internet via simple Asp.Net page.

The solution contains for projects
* PiFaceServer - *the part intended to run on the RaspberyPi*
* PiFaceClient - *the client used to talk to the server*
* PiFaceAPI - *simple Asp.Net page proxy that talks to the server*
* PiFaceClientTest - *a simple console act as a client and tests the server*

This is a product of 1 hour hackaton held a year ago, we recently demoed project and people wanted to see the code so here it is.

Created under time constraint, but still good as *HelloWorld* sample code.

When running under .Net in Visual Studio the server part will use a simple *emulator* that just based on time gives different I/O readings.

When running on the RaspberryPi via mono it will communicate to the PiFace via `WiringPi` lib, just place it in folder as `PiFaceServer` binary.

To obtain & compile the library via these steps:
```
git clone git://git.drogon.net/wiringPi
cd wiringPi
./build
```
Read more at http://wiringpi.com/