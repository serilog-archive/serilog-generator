serilog-generator [![Build status](https://ci.appveyor.com/api/projects/status/769o3i4sws3him7r/branch/master?svg=true)](https://ci.appveyor.com/project/serilog/serilog-generator/branch/master)
=================

A simulation that generates simple log data through Serilog, ideal for testing sinks or log servers.

![Screenshot](/asset/screenshot.png?raw=true)

The goals are that:

* We create higher-quality sinks and servers for Serilog by exercising them comprehensively
* We make it easier to build and test new sinks, without having n-different `Example.*` projects in the Serilog solution

How do I use it?
----------------

These instructions assume that you're building a sink for Serilog. Most of the time these live in their own DLL, and provide an extension method or two that hooks into the `.WriteTo.` Serilog configuration DLL.

As an example, here's how to fire up the generator against the `Serilog.Sinks.CouchDB` assembly.

### 1. Install the package

At the _Package Manager Console_:

```
PM> install-package serilog-generator
```

This is a 'tools' package, so it won't change assembly references.

### 2. Set the Start Action

1. In Visual Studio, right click the project you're working on (e.g. `Serilog.Sinks.CouchDB`) and select _Properties_.
2. In the _Debug_ tab select _Start External Program_
3. Navigate to and select the `packages/serilog-generator.0.0.0/tools/serilog-generator.exe` file
4. Under _Start Options > Command-line arguments_ point the program to your sink assembly and provide arguments **(see below)**
5. Finally, for sanity set the _Working directory_ explicitly to your output folder, e.g. _bin/Debug_

The `serilog-generator.exe` program accepts command-line arguments indicating the assembly file and sink methods to use. A full command line might look like:

```
serilog-generator --using="Serilog.Sinks.File" --write-to:File.path="test.txt"
```

Or on .Net Core (*nix)

```
dotnet serilog-generator.dll --using=\"Serilog.Sinks.File\" --write-to:File.path=\"test.txt\"
```

* **`--using`** - Provide either an assembly name, or the path to an assembly file.
* **`--write-to`** - The syntax of this command is _Method.parameter_; the parameter part is optional if no parameters need to be supplied to the method; specify `--write-to` once for each required parameter.

So, the command-line above is the same as:

```csharp
.WriteTo.CouchDB(databaseUrl: "http://my-couch/")
```

(When you enter this into the _Visual Studio_ project settings in step (4), don't include the `serilog-generator` executable part, since that's specified in step (3).)

The semantics match those of the `Serilog.Extras.AppSettings` [package](https://github.com/serilog/serilog/wiki/AppSettings). The command-line syntax is very strict; use double-dashes, quotes and so-on exactly as shown above (we'd like to improve this!).

### 3. Debug your project

Now, if you set your DLL project as the start-up project and press `F5`, the generator will start writing events to the sink.

The simulation is very simple right now - multiple threads run "actors" (e.g. customers) interacting with an "e-Commerce" site. Initially events will be logged slowly; over time the logging rate will ramp up. There's no limit to how much data will be created, reaching 1000s of events/second is possible, so don't leave it running and expect to come back to any disk space after a long coffee! :)

Get involved!
-------------

There are still some challenges to coming up with a good commmand-line syntax (and processor).

This project is **up-for-grabs** - feel free to jump in and make it happen. Just drop us a line via [Jabbr](https://jabbr.net/#/rooms/serilog) with an outline of what you're planning, so we don't collide.


