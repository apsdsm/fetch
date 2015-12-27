# Fletch

Fletch is a system of components that allow you to decouple your components in Unity, or at least make them not so *tightly* coupled.

It includes a lightweight IOC Container, which allows you to abstract services out to interfaces that will be resolved at run time, and also a Registry service that allows individual objects to store references to themselves to be picked up by other objects later on.

The tools in Fletch were designed to facilitate a specific Unity coding style that I've been using lately, so it's not really out there to implement everything you might want to see in an IOC Container or Registry component. If you'll like an IOC project for Unity that is much more full featured and mature, then I suggest you check out [StrangeIoC](http://strangeioc.github.io/strangeioc/), which does way more things than Fletch sets out to achieve.

# The IOC component

The IOC container allows MonoBehaviours to resolve their dependencies on services by querying the interface type, rather than the specific type. That is, rather than MonoBehaviours doing a `Find()` on specific object types, or calling on specific singleton instance of concrete classes, they call `Resolve<InterfaceType>()` on the IOC, which will provide a reference to whatever service implements that interface.

In more concrete terms, Fletch provides you with an IOC component that you can attach to a game object in your scene. You can then add any services you want to be made available as children game objects to the IOC object. For example:

```
IOC
 |- InputService
 |- AudioService
 |- ScoreService
```

Each service *must* implement a interface whose name ENDS with "Service". For example, the InputService object above should have an attached MonoBehaviour which implements something like IInputService.

When a scene starts, the IOC class will look at child objects, and add anything that implements an interface ending with `Service` to an internal directory. You can then access a reference to those services by asking the IOC class for the interface name in the **Start** method of a MonoBehaviour.

For example:

```csharp

void Start() {
    // get a reference to the input service
    InputService input = (InputService)IOC.Resolve<IInputService>();
}
```

Keep in mind that the ordering is important - Fletch does most of its work during **Awake**, so if you try access the services during your Mono Behaviours *own* Awake call, then results will be unpredictable (Fletch might not have had a chance to add its own components to the list.

I do want to ideally fix this so that Fletch can be bootstrapped by any call made during Awake, but for now calling during this phase is not recommended.

Putting references to your services in an IOC container allows you to set up your game objects so that they're working with interfaces of your services, rather than directly referencing the current implementation. This cleans up your service location quite a lot, and keeps your objects less tightly coupled to one another - you can replace the whole input service class used in your game by putting a new input service object in the IOC, rather than changes the references in all your objects.

It also means you don't have to fall back on the Singleton pattern so much in your code, such as setting a monolithic `InputService` class, with a static `GetInstance()` method.

This will make your code easier to test, because you can substitute your services with fakes when running integration testing. It also makes your code easier to maintain for the reasons above (with the added bonus that you don't need a singleton in your code).

Before you ask, the IOC itself is *not* a static class. It does implement some static methods, but these are actually facades for the instantiated version - because we need to draw a line at least somewhere otherwise the performance gains of using a service locator like this are rendered mute by having to call `Find` each time we use it.

True enough, currently you can only really have one active IOC in a scene, but it should be possible in a future release to have multiple IOCs, and reference them by name (not that I see any performance gains from doing so).

# The Registry Service

Fletch comes with a Registry Service component that can be added to your projects. The registry service is an object that will allow the other objects in your scene to `Register()` themselves, and then for other objects to get a reference to that object by performing a `LookUp()`.

This means that you can share references to objects without those objects ever needing to communicate directly. In fact, all they really need to know is the identifier and type of the object they want to look up. The type can also be an interface type, which will help keep the coupling between objects even more loose.

Finally, the Registry allows objects to *reserve* a reference to an object that *hasn't been registered yet*. This is actually pretty cool and useful. It allows objects to declare that they need a certain object, and then carry on as usual. When that object finally registers with the Registry, the reservation will be fulfilled.

## To register an object with the Registry

```csharp

Foo foo = new Foo();

registry.Register( Foo.getType(), "Foo", foo);
```

## To lookup an object that was registered previously

```csharp
Foo lookupFoo = registry.Lookup<Foo>( "Foo" ); 
```

## To reserve a reference

When reserving a reference, the reserving class needs to contain a setter with the same name as the object that is being reserved.

```chsarp
class Foo : MonoBehaviour
{
	private Bar bar;
	private IRegistryService registry; 

	public Start()
    {
        registry = IOC.Resolve<IRegistryService>();
    }

	public ReservableBar
    {
        set { this.bar = value; }
    }

	void MakeReservation()
    {
	    registry.Reserve<Bar>( "ReservableBar", this );
    }

    void Update()
    {
        if (bar != null)
        {
            /* ... */
        }
    }
}
```

In the code above, a reservation can be made for a component called "ReservableBar" of type `Bar`. When that registration takes place, the Registry will call the public setter on `Foo` and provide the new data.

To make a reservation you *must* provide a public setter with the same name or the Registry will throw an exception.

# Currently usable?

I'm currently experimenting with the best way to do certain things. As such, I recommend not using Fletch in any projects currently - when it's at a point where I can use it in my own projects I'll write more extensively about best practices, etc, so for now it's on GH more as a place holder, and in order to cultivate discussion.

## Future plans

I'd like to add more functionality to Fletch, without overstepping the bounds of its initial mission (just providing references to abstracted implementations). Mostly this will occur as I find myself requiring more functionality. But of course I'm also open to expanding the library to suit the needs of others.