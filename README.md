# BuzzIO

![Buzz Controller](https://github.com/bbeardsley/BuzzIO/raw/master/icon.png) This is a C# library to use Sony PS2 Buzz controllers in Windows.

## Usage
```C#
var handsets = new BuzzHandsetFinder().FindHandsets().ToList();
```

## Links
- [BuzzIO in nuget gallery](http://nuget.org/packages/BuzzIO)
- [Working with USB devices in .NET and C#](http://www.developerfusion.com/article/84338/making-usb-c-friendly/): Inspiration and start of this library
- [Buzz Demo Source Code](http://www.developerfusion.com/resource/download/content/84338/buzz%20demo%20source%20code/): Original source code
- [How to get the Wireless Buzz! Controllers working on a PC](http://www.soundtherapy.org.uk/253/wireless-switch-article/): Good tips on debugging, etc
- [Vendor and Device Ids](http://www.linux-usb.org/usb.ids)
