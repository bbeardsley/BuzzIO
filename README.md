# BuzzIO

[![Nuget](https://img.shields.io/nuget/v/BuzzIO.svg)](https://nuget.org/packages/BuzzIO)
[![Build Status](https://img.shields.io/appveyor/ci/bbeardsley/BuzzIO.svg)](https://ci.appveyor.com/project/bbeardsley/BuzzIO/history)

## What Is It?
![Buzz Controller](https://github.com/bbeardsley/BuzzIO/raw/master/icon.png) This is a C# library to use Sony PS2 Buzz controllers in Windows.

## Usage
```C#
var handsets = new BuzzHandsetFinder().FindHandsets().ToList();
```

## Links
- [Working with USB devices in .NET and C#](http://www.developerfusion.com/article/84338/making-usb-c-friendly/): Inspiration and start of this library
- [Buzz Demo Source Code](http://www.developerfusion.com/resource/download/content/84338/buzz%20demo%20source%20code/): Original source code
- [Vendor and Device Ids](http://www.linux-usb.org/usb.ids)
