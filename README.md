# Umbraco.DataAnnotations (Umbraco 10 only)

Contains model validation attributes to for your properties, by using umbraco dictionary as the resource for error messages.

## Big thanks to rasmuseeg
The original project that seems to be no longer active can be found below, my work is based on his.
https://github.com/rasmuseeg/Our.Umbraco.DataAnnotations

## Installation
This version will only work with Umbraco 10. Any lower version will not work!
During installation the keys will be created nested below `DataAnnotations` dictionary key.

Build the project and start website.
On first run, a migration will check foreach dictionary key used in the application and added it to umbraco dictionary items.
Only default `en-US` keys and translations are added.

NuGet:
```
PM > Install-Package Custom.Umbraco.DataAnnotations
```

Build the project and start website.
On first run, a migration will check foreach dictionary key used in the application and added it to umbraco dictionary items.
Only default `en-US` keys and translations are added.

## Client Validation
Include the following scripts in your layout .cshtml file

```
<body>
    @RenderBody()

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.5/jquery.validate.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/4.0.0/jquery.validate.unobtrusive.min.js"></script>
</body>
```

The above is just samples, you may use any method you like to include the scripts. You could create a Partial View to include these scripts (except JQuery if you need that globally) to enable client side validation for specific pages only.

### 

## Attributes
Decorate your properties with the following attributes

 * UmbracoCompare
 * UmbracoDisplayName
 * UmbracoEmailAddress
 * UmbracoMinLength
 * UmbracoMaxLength
 * UmbracoStringLength
 * UmbracoMustBeTrue
 * UmbracoRegularExpression
 * UmbracoRequired
 * UmbracoRemote

Not setting a custom key, will fallback to the default dictionary key.
