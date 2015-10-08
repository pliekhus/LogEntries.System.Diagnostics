# Logging To Logentries.

Logentries currently has plugins for [NLog](#nlog) and [Log4net](#log4net).  This project adds the System.Diagnostics Listener to that list.

To configure your application to log to Logentries, you will need to perform the following tasks:

1. Create a Logentries account.
2. Create a host and log to receive your log data.
3. Adding the Plugin library and the appropriate Logentries Appender libraries to your application.
4. Configure the Plugin and the Logentries appender in your application.
5. Send log messages from your application.

These steps are outlined in further detail below.

## Creating Your Logentries Account

You can register your account on Logentries by browning to [https://logentries.com](https://logentries.com) and simply clicking `Sign Up` at the top of the page.

## Creating the Host and Log

Once logged in to your Logentries account, create a new host with a name that best represents your application. Select this host and create a new log with a source type of `Token TCP` (see below for more information) and a name that represents what you will be logging.

Please note that Logentries reads no particular meaning into the names of your hosts and logs; they are primarily for your own benefit in keeping your logs organized.

## Adding the Logentries Plugin Libraries to Your Application

System.Diagnostics
------------------
The easiest way to add the Log4net and the Logentries Plugin library to your application is to install the `logentries.sysdiagnostics` [Nuget package](http://www.nuget.org/packages/logentries.sysdiagnostics "Nuget package"). This package will install the Logentries Plugin library.

If you would rather install the Logentries appender manually, you can download the complete code in this GitHub repository, compile the Diagnostics.LogEntries Visual Studio project within it into a DLL file, and then reference this file in your application. If you choose this option you must install Log4net yourself.

The Logentries appender is configured and added to your app.settings and system.diagnostics configuration in the normal way:

```xml
 <appSettings>
    <add key="LogEntries.Token" value="00000000-0000-0000-0000-000000000000"/> <!--Your LogEntries.com key goes here-->
    <add key="LogEntries.ImmediateFlush" value="true"/>
    <add key="LogEntries.Debug" value="false"/>
  </appSettings>
  <system.diagnostics>
    <trace>
      <listeners>
        <clear />
        <add name="LogEntries" type="Diagnostics.Listeners.LogEntriesListener, Diagnostics.Listeners" />
      </listeners>
    </trace>
  </system.diagnostics>
```

### Logentries Credentials

Logentries credentials determine to which host and log your log messages are sent. The following settings constitute the Logentries credentials:

- **Token**: The unique token GUID of the log to send messages to. This applies when using the newer token-based logging.

```xml
 <appSettings>
    <add key="LogEntries.Token" value="00000000-0000-0000-0000-000000000000"/> <!--Your LogEntries.com key goes here-->
    <add key="LogEntries.ImmediateFlush" value="true"/>
    <add key="LogEntries.Debug" value="false"/>
  </appSettings>
```

## Shutting Down the Logger

The Logentries target keeps an internal queue of log messages and communicates with the Logentries system using a background thread which continuously sends messages from this queue. Because of this, when an application is shutting down, it is possible that some log messages might still remain in the queue and will not have time to be sent to Logentries before the application domain is shut down.

To work around this potential problem, consider adding the following code to your application, which will block for a moment to allow the Logentries appender to finish logging all messages in the queue. The AreAllQueuesEmpty() blocks for a specified time and then returns true or false depending on whether the queues had time to become empty before the method returns.

You can use the LogEntriesListener static method ShutdownLoggin to wrap this for you.

```c#
public void Application_End()
{
	LogEntriesListener.ShutdownLogging();
}
```
