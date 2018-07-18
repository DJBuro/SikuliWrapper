# SikuliWrapper

SikuliWrapper is C# based wrapper is based on popular Python automation tool. It use basic console commands provided by SikuliApi for executing basic action on Screen

  - Click
  - Type
  - Hover
  - Double click
  - Right click
  - Drag and Drop

You can also:
  - Assert if element exist on the Screen
  - Wait for elements to show on screen


## Installation

There is nuget package, which you ca download from package manager. SikuliApi need Java installed on machine (there is bug in SikuliApi and it dont work with Java 10. So for now keep with Java 8 or 9).

| Software | Download |
| ------ | ------ |
| Java | [https://java.com/en/download/][PlDb] |

## Using

In this wrapper you have to know about two object types, which will be absolutelly enough for using this package.

### Image

SikuliApi searches for images on the screen. You can create them like this: 

```csharp
string path = "c:\test.png"
IImage testButton = Image.FromFile(path);
```

SikuliApi check for image on screen and compare it pixel by pixel. No one can guarantee pixel perfect images all the time, because of this images have similarity. Default similarity is 70%, but it is configurable when creating new image, by adding second parameter with double value between 0 and 1.

```csharp
string path = "c:\test.png"
IImage testButton = Image.FromFile(path, 0.8);
```

### Screen

Screen object is most important one. By creating screen object you start java and sikulix processes, so it is important to dispose this object after it's job is done. It implements IDisposable interface, so you can use it like this: 

```csharp
using (var screen = Sikuli.CreateSession())
{ }
```

All basic operation are methods for screen. You need instance of screen for invoke every method: 

```csharp
using (var screen = Sikuli.CreateSession())
{
	screen.Click(testButton);
	screen.RightClick(testButton);
	screen.DoubleClick(testButton);
	screen.Hover(testButton);
}
```

There are two specific methods on screen, which are used just for checking if the element is present on screen. These methods can be use like Asserts in our test. 
  - Exist - check if current image is present on screen
```csharp
	screen.Exists(testButton);
```
  - Wait - check every 500 ms if element is on the screen for specific period of time. 
```csharp
	screen.Wait(testButton, 4);
```

Period is maximum time for waiting, if element present before wait time end, Sikuli will continue. Every single operation, wait by default 2 seconds to check for element.




**Free Software, Hell Yeah!**

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)


   [dill]: <https://github.com/joemccann/dillinger>
   [git-repo-url]: <https://github.com/joemccann/dillinger.git>
   [john gruber]: <http://daringfireball.net>
   [df1]: <http://daringfireball.net/projects/markdown/>
   [markdown-it]: <https://github.com/markdown-it/markdown-it>
   [Ace Editor]: <http://ace.ajax.org>
   [node.js]: <http://nodejs.org>
   [Twitter Bootstrap]: <http://twitter.github.com/bootstrap/>
   [jQuery]: <http://jquery.com>
   [@tjholowaychuk]: <http://twitter.com/tjholowaychuk>
   [express]: <http://expressjs.com>
   [AngularJS]: <http://angularjs.org>
   [Gulp]: <http://gulpjs.com>

   [PlDb]: <https://github.com/joemccann/dillinger/tree/master/plugins/dropbox/README.md>
   [PlGh]: <https://github.com/joemccann/dillinger/tree/master/plugins/github/README.md>
   [PlGd]: <https://github.com/joemccann/dillinger/tree/master/plugins/googledrive/README.md>
   [PlOd]: <https://github.com/joemccann/dillinger/tree/master/plugins/onedrive/README.md>
   [PlMe]: <https://github.com/joemccann/dillinger/tree/master/plugins/medium/README.md>
   [PlGa]: <https://github.com/RahulHP/dillinger/blob/master/plugins/googleanalytics/README.md>
