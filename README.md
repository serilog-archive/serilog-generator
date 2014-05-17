serilog-generator
=================

A simulation that generates simple log data through Serilog, ideal for testing sinks or log servers.

![Screenshot](/asset/screenshot.png?raw=true)

The goals are that:

* We create higher-quality sinks and servers for Serilog by exercising them comprehensively
* We make it easier to build and test new sinks, without having n-different `Demo.*` projects in the Serilog solution

This is still in the early stages, the plan is to ship two packages: a library via NuGet containing the "simulation", and an executable `serilog-generator.exe` via Chocolatey.

The executable will support the same options as the `Serilog.Extras.AppSettings` [package](https://github.com/serilog/serilog/wiki/AppSettings), along the lines of:

```
serilog-generator --write-to:RollingFile.pathFormat="C:\Logs\myapp-{Date}.txt"
```

There are still some challenges to coming up with a good commmand-line syntax (and processor).

This project is **up-for-grabs** - feel free to jump in and make it happen. Just drop us a line via [Jabbr](https://jabbr.net/#/rooms/serilog) with an outline of what you're planning, so we don't collide.


