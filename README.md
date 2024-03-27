# ngettext-wpf
Proper internationalization (i18n) support for WPF (via NGettext)

Review the <a href="CHANGELOG.md">change history</a>, including whats new in the latest release and what will be included in the upcomming release.

[//]: <> (TODO: Lookup shields.io badges for nuget package versions, etc.)
[//]: <> (TODO: Could also perhaps automate not only build but also unit tests passing/failing.)

[![Build status](https://ci.appveyor.com/api/projects/status/s344j6n3gpvjxjof?svg=true)](https://ci.appveyor.com/project/robert-j-engdahl/ngettext-wpf)

![LGPL](https://www.gnu.org/graphics/lgplv3-88x31.png)

## Getting Started
[//]: <> (TODO: Needs work, also attention re: CompositionRoot.Compose.)
[//]: <> (TODO: We think NGettext does support 'domains' which is a good thing; need to verify that fact.)
[//]: <> (TODO: Further we think that domain-based contexts should be able to support isolation, containment.)
[//]: <> (TODO: i.e. for use cases, such as per Window/Control, enum values, etc)

Get the NuGet from here <a href="https://www.nuget.org/packages/NGettext.Wpf/">https://www.nuget.org/packages/NGettext.Wpf/</a>.

NGettext.Wpf is intended to work with dependency injection.  You need to call the following at the entry point of your application:

```c#
NGettext.Wpf.CompositionRoot.Compose("ExampleDomainName");
```

The `"ExampleDomainName"` string is the domain name.  This means that when the current culture is set to `"da-DK"` translations will be loaded from `"Locale\da-DK\LC_MESSAGES\ExampleDomainName.mo"` relative to where your WPF app is running (You must include the .mo files in your application and make sure they are copied to the output directory).

Now you can do something like this in XAML:

```xml
<Button CommandParameter="en-US" 
        Command="{StaticResource ChangeCultureCommand}" 
        Content="{wpf:Gettext English}" />
```
Which demonstrates two features of this library.  The most important is the Gettext markup extension which will make sure the `Content` is set to the translation of "English" with respect to the current culture, and update it when the current culture is changed.  The other feature it demonstrates is the `ChangeCultureCommand` which changes the current culture to the given culture, in this case `"en-US"`.

Have a look at <a href="NGettext.Wpf.Example/UpdateTranslations.ps1">NGettext.Wpf.Example\UpdateTranslations.ps1</a> for how to extract msgids from both xaml and cs files.  

Note that the script will initially silently fail (i.e. 2> $null) because there is no .po file for the given language.  In the gettext world you are supposed to create that with the <a href="https://www.gnu.org/software/gettext/manual/html_node/Creating.html">msginit</a> command that ships with the <a href="https://www.nuget.org/packages/Gettext.Tools/">Gettext.Tools</a> nuget package, or PoEdit can be used to initialize the catalog from the intermediate pot file created.  Here is what recently worked for me:

```
PM> mkdir -p Locale\en-GB\LC_MESSAGES\
PM> msginit --input=obj\result.pot --output-file=Locale\en-GB\LC_MESSAGES\ExampleDomainName.po --locale=en_GB
```

---

## Conventions
[//]: <> (Why 'Locale'? <LOCALE>? LC_MESSAGES? I think I see the historical precedent for having done that, but still...)

Keep your compiled translations in `"Locale\<LOCALE>\LC_MESSAGES\<DOMAIN>.mo"`.  This library will force you to follow this convention.  Or rather, NGettext forces you to follow a convention like `"<PATH_TO_LOCALES>\<LOCALE>\LC_MESSAGES\<DOMAIN>.mo"`, and I refined it.

Keep your raw translations in `"Locale\<LOCALE>\LC_MESSAGES\<DOMAIN>.po"`.  This is not enforced, but when working with POEdit it will compile the `".mo"` file into the correct location when following this convention, and it doesn't remember your previous choice, so stick with the defaults.

There are lots of GNU conventions related to I18N and localization (l10n).  One of them is that the notion that the original program is written in US English, so you don't need to translate anything to facilitate I18N.  The original text in US English is called the `msgId`.

One of the most important GNU convention related to I18N is providing a context to the translaters so they have a chance to do it right.  For instance the English word 'order' has a number of more or less related meanings and thus different translations.  For instance in the context of sequential ordering, 'order' translates to 'rækkefølge' in `da-DK`, but the imperative for placing an order translates to 'bestil'.  Here is an example of how that can be fixed:

```xml
<!-- A button with the text 'Order' but with a helpful context for the translators -->
<Button Command="{StaticResource PlaceOrderCommand}" 
        Content="{wpf:Gettext Imperative for placing an order|Order}" />
```

Translaters will rarely think of it, and just translate the first meaning that comes to mind, and as a programmer you might not know which words or sentenses needs a context, I therefore highly recommend to always provide a helpful context.

---

## Support

Reach out to me at one of the following places!

- Twitter at <a href="https://twitter.com/robert_engdahl" target="_blank">`@robert_engdahl`</a>
- LinkedIn at <a href="https://www.linkedin.com/in/robertengdahl/" target="_blank">`robertengdahl`</a> 
- Create a question on <a href="https://stackoverflow.com/questions/ask?tags=ngettext.wpf">Stack Overflow</a>.  Use the tag `ngettext.wpf`.
- or create an <a href="https://github.com/robert-j-engdahl/ngettext-wpf/issues">issue</a>

---

## Sample Application
[//]: <> (TODO: we can stand to think about the example, and breakout out a more 1C proper view model.)
[//]: <> (TODO: contrasted with actual Windows INPC properties, which is kind of counter to the MVVM architecture.)

In <a href="NGettext.Wpf.Example/">NGettext.Wpf.Example/</a> you will find a sample application that demonstrates all the features of this library.

![Demo](demo.gif)

