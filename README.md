# Service Monitor

This little Windows application sits in your tray and alerts you if a configured service goes down.

Supports custom plugins.

## Usage

Simply run the application. A grey dot will appear in the tray icon area.
Double clicking on the dot or using the "Show" context menu item will bring up the main form.

In the main form you have an overview of all tests as well as the ability to create/edit/delete tests.

The tray icon dot changes depending on the state:

- Grey: No tests or all tests are disabled
- Red: At least one test is failing
- Green: All tests pass

### Main form

The main form shows an overview of all tests and allows configuration of new/existing tests.

Use the "Add Plugin" menu option to add a new test.
You can double click a test in the list to edit it.
A context menu is provided for further options.
Changes you make are automatically and immediately saved to a configuration file
that resides in the same directory as the executable.

The icon color gives a status hint:

- Grey: Test is disabled
- Green: Last test run passed
- Yellow: Test is being performed
- Red: Last test run failed

The "File" menu provides an option to re-run all tests immediately.

Note: Closing the form does not exit the application.
Use the "Exit" option in either the "File" menu or the tray icon context menu to fully exit the application.

## Included plugins

The application comes with a few useful plugins included.

### ICMP Ping

This plugin simply pings a host and throws an error if the ping failed.

### Service status

This monitors a local Windows service and alerts you if the service status (running, stopped, etc)
is not in the expected state.

### TLS Certificate check

This tests the certificate of a TLS connection.
The plugin alerts you if basic checks (such as name validation) fails,
or if the expiration time is closer than a configured time window.

### HTTP Status

This makes an HTTP request and checks if the status code of the response matches the configured value.

# Custom plugins

You can create custom plugins.
Creating a plugin is fairly simple and straightforward.
Plugins are provided in the form of DLL files.
A single DLL file can contain multiple plugins.

## Storage

Custom plugins must be stored in a directory called "Plugins" in the application directory.
The individual plugin directory name must match the DLL file name.

**Example:**

The application is at `C:\Monitor\ServiceMonitor.exe`.
This means plugins are in `C:\Monitor\Plugins`.
A "FileSystem" plugin would thus be located at `C:\Monitor\Plugins\FileSystem\FileSystem.dll`

Plugins not stored in this form will be ignored.

## Naming convention

When a DLL file is scanned for plugins,
the scanner looks for type names that end in "Plugin".
Only public classes are searched.
Structs and other types are skipped even if they meet the naming convention.
A plugin must thus be declared as `public class ExamplePlugin {}`

The name check is case sensitive.
"Exampleplugin" is not matched, but "ExamplePlugin" is.

## Plugin configuration

The plugin configuration is stored in the same directory as the main executable.
The file is called `config.bin`

## Plugin ordering

Internal plugins are always scanned first, and as such always appear at the top of the plugin list.

In the future, internal and external plugins may be split visually as well.
Plugins may also be grouped by DLL files if naming conflicts start to get common.

## Implementation

- Your plugin must implement the `IDisposable` interface, even if the Dispose method is empty
- Your plugin must implement a constructor that takes no arguments

Apart from that, there are no interfaces or other dependencies you need to have.

## Members

Your plugin must implement the following public members:

| Member Type | .NET type | Name         | Info                   |
|-------------|-----------|--------------|------------------------|
| Method      | void      | Start()      |                        |
| Method      | void      | Stop()       |                        |
| Method      | bool      | Config()     |                        |
| Method      | void      | Load(byte[]) |                        |
| Method      | byte[]    | Save()       |                        |
| Method      | void      | Test()       |                        |
| Method      | void      | Dispose()    | From IDisposable       |
| Property    | string    | BaseName     | Static                 |
| Property    | string    | Name         | Property can be static |
| Property    | Date      | NextCheck    |                        |
| Property    | string    | LastStatus   | Property is optional   |

All of the properties need a public getter.
A public setter is not required, as these are never set externally by the monitor.

## Thread safety

The monitor makes no guarantee to thread safety.
The only guarantee is that no single function is run multiple times in parallel.

To ensure thread safety, it's a good idea to make a copy of the setting values right at the start of `Test()`.
This way you won't negatively impact the test run if the plugin is configured while a run is occuring.

You want to especially be prepared for calls to any other function (including `Stop()` and `Dispose()`)
while `Test()` is running.

A legitimate way to resolve these problems is to throw an exception from within `Test()`.
This causes a regular test failure.

## Method Start()

This is called before `Test()` is called for the first time,
or when a user enables a plugin.
It should set `LastStatus` to null and calculate `NextCheck`.

Your plugin should not crash when this is called multiple times.

## Method Stop()

This is called before the plugin is destroyed,
or when it has been disabled by the user.
This should set `LastStatus` to null.

Your plugin should not crash when this is called multiple times, or after `Dispose()` has been called.

## Method Config()

This is called when the user wants to configure the plugin.

Configuration currently means showing a form that offers all options the user can set.
Because of that, you have the guarantee that this function will always be called on the UI thread.

This function should block the current thread until configuration is completed or cancelled.
You should thus use the `.ShowDialog()` of your configuration form and configure the `DialogResult`
of your OK and Cancel button appropriately.

The return value of this function decides whether configuration was successful or not.
The configuration should only be reported as successful if all values are valid,
because the monitor will immediately call `Save()` if it is.

False means the configuration was not successful, for example because the user cancelled the dialog.
In that case, the plugin is not saved.
In case of a new plugin creation, the new plugin is disposed.

Note: During configuration of an existing plugin,
the application may continue to call `Test()` due to `NextCheck` elapsing.
Because of that you should make sure the plugin is always in a valid state.
The Builtin plugins achieve this by loading a dummy shadow plugin into the configuration form,
and only upon success overwrite all configured values in the real plugin.

## Method Load(byte[])

Restores a plugin from serialized data that was previously generated using `Save()`.
Note: the argument to `Load(byte[])` will never be null,
even if you returned null in a previous call to `Save()`.
Instead, `new byte[0]` is used.

The supplied data is guaranteed to originate from your plugin.
Identification is performed by comparing the fully qualified name of your plugin
against the fully qualified name stored on file.

This also means if you rename your plugin or namespace the configuration will get lost.

### Forward compatibility

It's recommended that you implement a way to achieve forward compatibility,
for example by serializing a magic number or version number.

Do not make the call to `Load()` fail if a new version of your plugin requires more configuration,
instead make the load succeed and then make the call to `Test()` fail with a descriptive error.
If possible, supply sensible default values for newly added options,
and migrate removed options.

### Load failure

If your plugin fails to load it is silently discarded.

## Method Save()

This is called by the monitor when the plugin configuration needs to be saved.
The exact format can be freely chosen by your plugin.
Recommendation is a `MemoryStream` with a `BinaryWriter`
that writes down all config values in order (see code for builtin plugins).

When the plugin is loaded from the file at a later time,
the value returned in this function will be passed to `Load(byte[])`

If your plugin lacks configuration data, return either `null` or `new byte[0]`.
These two values have the same effect.

### Storing sensitive information

The monitor will encrypt your data.
Currently this is done using the `CryptProtectData` functions.
This means you do not need to encrypt sensitive data yourself.

The data will automatically be decrypted by the monitor when plugins are loaded.

## Method Test()

Tells the plugin to perform the configured test.
This function must block the thread until the test completes.
Your plugin should not interact with the UI from within a test because it runs in a separate thread.
You should also program timeouts into your plugin,
because if there's a way for a plugin to get stuck indefinitely,
the state will forever be stuck in the yellow testing state in the UI when this happens,
and `Test()` is never called again.

**Important**: Do not make plugin tests depend on other plugin tests.
Each test is run in its own thread, an all tests can potentially run in parallel
if they have the same `NextCheck` value.

To signal a failed test, throw an exception of your choice.
The exception message will be shown in the main window,
and a notification is triggered whenever the plugin switches between a success or error state.

Note: It's recommended you pick a sensible exception type (or even create your own).
A logging facility may be added in the future that logs the type name.

To communicate an optional result on successful runs,
use the `LastResult` property (see further below).
If you don't do this, the result will simply show "OK".

### Updating the test schedule

Whenever this function is called,
one of the first tasks you should do
is setting the `NextCheck` value to the time of the desired **next** call to `Test()`.

The monitor will never run multiple tests of the same plugin in parallel,
even if `NextCheck` is in the past.
If the value is in the past, `Test()` will be called on the next idle second.

Tip: The user should be able to configure how frequently the test runs (see `Config()`),
however, the monitor will not enforce a static interval.
You can set `NextCheck` much closer to the current time
if a check failed to decrease the check interval on errors.
You can see this in use in the TLS plugin, which normally has very long intervals.

## Method Dispose()

This is from the `IDisposable` interface.
The function can be empty if your plugin doesn't depends on the interface, but it must be declared.

Calling this function should ideally abort any still running test.

Note:

- This function may be called multiple times and should never crash.
- This function may be called without `Stop()` being called first.
- This function must never throw an exception.

## Property BaseName

This is a static property that should hold the name of the plugin.
This name is displayed in the "Add Plugin" main form menu.

You should make sure this name is unique,
otherwise the user will have duplicate names in the plugin list without knowing which name is which plugin.

In other words, do not just name a plugin `HTTP`
unless it really does everything a user could ever want from a HTTP check plugin.
If your plugin checks a json response, name it `HTTP json response` or similar.
For this reason, the internal HTTP plugin is named `HTTP status`,
because it only checks the status code of the response.

## Property Name

This is the name of the plugin that is displayed in the active plugin list.
This property can be either an instance property or a static property.

A static property is useful if you do not plan on changing the plugin name.

An instance property is useful if you want to display a more personalized name.
This makes most sense if you expect the user to potentially load your plugin multiple times.

All internal plugins do this.
The service check plugin for example displays the service name,
the ICMP plugin displays the configured host name, etc.

If you do not plan on using this, you can create a declaration as simple as

`public static string Name => BaseName;`

The name in the loaded plugin list is not very frequently updated.
Do not rely on changes to this property being reflected in the UI instantly.

The name is guaranteed to update on successful calls to `Config()`.

## Property NextCheck

This property should point to the time you want the monitor to call `Test()`.
The value is ignored:

- until `Start()` is called
- while a test is running

Note that ignored doesn't mean it's not read at all,
but rather that the value is discarded.

This property is currently evaluated once per second.
`Test()` is called once it's no longer in the future,
provided a test is not already ongoing.
This permits you to change the property at any time,
or add logic to the property that sets it to the past
if there are good reasons to run a test sooner than configured.

Note: Do not depend on the fact that this property is checked once per second.
The user may be allowed to set a minimum delay between tests,
which would have an effect on this interval.
In the future, this property may be checked less frequently, or irregularily.

## Property LastStatus

This property is optional and you do not need to declare it at all
if you do not intend to use this mechanic.
The Builtin HTTP status check plugin for example does not declare this property.

It allows you to convey additional information on success.
If the property is absent, the value "OK" is displayed as the status.
If the property is present, the property value is instead shown.

The ICMP ping plugin uses this mechanic to show you the IP address that responded,
as well as the time it took for the response to arrive.

The property is only evaluated when the last call to `Test()` was sucessful.
If the last call failed, the error message of the exception is shown instead.
This means you do not need to reset the `LastStatus` property when the test fails.
