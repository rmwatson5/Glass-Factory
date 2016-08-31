# Glass Factory

While GlassMapper may be an excellent ORM for pulling out Sitecore Items, there are many shortcomings that I have found. The aim of this project is to act as an extension to GlassMapper for Sitecore. 

## Setup
This solution just has a single DLL that it outputs. You will need to provide references to the following packages:

* BoC.Glass.Mapper.Sc.CastleWindsor
* Boc.Glass.Mapper.Sc.Core
* Castle.Core
* Castle.Windsor
* Glass.Mapper
* Glass.Mapper.Sc
* Glass.Mapper.Sc.Core
* Glass.Mapper.Sc.Mvc-5
* Microsoft.AspNet.Mvc
* Microsoft.Web.Infrastructure
 
NOTE: If you wish to provide the name of the assembly where all of your models are stored for Glass Factory to look, add the assembly path in your app config with the name of "GlassModel"

### How to use:
The setup for Glass Factory is very similar to that of GlassMapper. It can be set up using dependency injection like so:
```ch
container.Register<IGlassFactory, GlassFactory>();
```

Otherwise, you can create a new instance of GlassFactory when pulling out a Glass Item like so:
```cs
IGlassFactory glassFactory = new GlassFactory();
var glassItem = glassFactory.GetGlassItem<IGlass_Item>("Path/To/Item");
```

## Solutions to problems found in GlassMapper:
### Solution 1: Uses of interfaces
GlassMapper is simple for pulling items out of Sitecore. If I wanted to get the contents of a Glass Item I could execute the following code:
```cs
GlassItem glassItem;
using (var context = new SitecoreContext())
{
    glassItem = context.GetItem<GlassItem>("Path/To/Item");
}
```
Now I can use a strongly typed model that I created to pull out content from Sitecore without having to use strings.

Lets, however, say I want to do some unit testing and decide to use the interface instead to pull out the item:

```cs
IGlassItem glassItem;
using (var context = new SitecoreContext())
{
    glassItem = context.GetItem<IGlassItem>("Path/To/Item");
}
```
I will run into a problem where GlassMapper will generate a proxy class to inherit from IGlassItem. But what if I wanted it to use GlassItem - a model I have already created in code that inherits from IGlassItem? That's what Solution 1 in Glass Factory tries to solve. 

Glass Factory will locate the first inherited type from the interface you supply to retrieve out the item and cast it as the interface. This way you can use any methods you may have created in your concrete type while still be able to create MOCK objects from the interface for Unit Testing.

### Solution 2: Object Inheritence 
One issue I have stumbled accross when using GlassMapper for Sitecore is its lack of using inheritence.

So let's say I have two items in Sitecore: Base Article, and News Article where News Article inherits from Base Article. I will then create the models and set up the inheritance in code. So now if I want to grab out a News Article out in code, I could do this:

```cs
var article = this.GlassFactory.GetGlassItem<INews_Article>("Path/To/News Article");
```
Cool! I can now use that item as I wish. But lets say I want to pull out that news article using its inherited type but still recognize the concrete class of News Article?

```cs
var article = this.GlassFactory.GetGlassItem<IBase_Article>("Path/To/News Article");
```
What happens here is that GlassMapper would generate a proxy class to inherit from IBase_Article. It will have no knowledge that this article is a News Article and casting would be impossible:

```cs
var article = this.GlassFactory.GetGlassItem<IBase_Article>("Path/To/News Article");
var field = (article as INews_Article).NewsArticleField; //This would generate an exception because article would be null
```

When an item is requested from Glass Factory, it will first pull out the Sitecore Item and check the template id, from there it will locate a concrete type that has an attribute with that template id. It will then use Glass Mapper to map all its properties to that type. Glass Factory will take that item and attempt to cast it as the interface provided in the generic paramater (assuming the concrete class inherits from the interface provided).


## Extra Methods
### Get Children
```cs
IEnumerable<TCi> GetChildren<TCi>(ID parentId, bool isLazy = false, bool inferType = true, bool deep = false, Func<TCi, bool> filter = null)
```

I have provided an easier way to get the child items of a Glass Item. This function has the ability to get all the children of the current item with the id provided and assign the types correctly to each child item.

Example:
```cs
var parentItem = this.GlassFactory.GetGlassItem<IParent_Item>("Path/To/Item");
var children = this.GlassFactory.GetChildren<IChild_Item>(parentItem.Id);
```
You can still reuse the paramaters provided by GlassMapper when getting the children items out (isLazy, inferType) along with twp other paramaters (deep and filter).

#### Deep (bool):
This paramater will tell Glass Factory to not only grab the children items but all of its descendants.

#### Filter (Func<TCi, bool>):
This allows you to filter which child items you want to come back. It is be included in the lazy load - it will only execute when called.

### Get Site Root
```cs
TCi GetSiteRoot<TCi>()
```
This is an easy way of getting the Siteroot of the current site you are on and map it to a Glass Model you provide

### Get Homepage
```cs
TCi GetHomePage<TCi>()
```
This is an easy way of getting the start path of your site and map the result to a Glass Model you provide

### Get Current Glass Item
```cs
TCi GetCurrentGlassItem<TCi>(bool isLazy = false, bool inferType = true)
```

This method will get the current page item using the Sitecore Context item. Glass Factory will then take that item and map it to the Glass Item you provided.

