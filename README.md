# Fetch

Fetch is a very light weight IOC Container for Unity. It allows you decouple your components in Unity, or at least make them not so *tightly* coupled.

Fetch was designed to facilitate a specific Unity coding style that I've been using lately, so it's not really out there to implement everything you might want to see in an IOC Container or Registry component. If you'll like an IOC project for Unity that is much more full featured and mature, then I suggest you check out [StrangeIoC](http://strangeioc.github.io/strangeioc/), which does way more things than Fetch sets out to achieve.

On the other hand, if StrangeIoC seems to do way too much, and all you need is a system for fetching your components by interface, then Fetch might be right for you.

## The IOC Container

The IOC container allows MonoBehaviours to resolve their dependencies on services by querying the interface type, rather than the specific type. That is, rather than MonoBehaviours doing a `Find()` on specific object types, or calling on specific singleton instance of concrete classes, they call `Resolve<InterfaceType>()` on the IOC, which will provide a reference to whatever service implements that interface.

In more concrete terms, Fetch provides you with an IOC component that you can attach to a game object in your scene. You can then add any services you want to be made available as children game objects to the IOC object. For example:

```
IOC
 |- InputService
 |- AudioService
 |- ScoreService
```

When a scene starts, the IOC class will look at child objects, and create a directory of references to each interface implemented by each of the child's components, and then link the reference back to the component itself. You can then access that directory by asking the IOC class for the interface name in the **Start** method of a MonoBehaviour.

For example:

```csharp

void Start() {
    InputService input = IOC.Resolve<IInputService>();
}
```

You could also call this from Awake. When Fetch receives the first call to resolve a component, if it has not already built the reference directory it will do so at that time.

*Note that since Fetch provides a static reference, you can resolve services using the `IOC.Resolve` method without having to first resolve any instance of the IOC.*

## Why should you do this?

Putting references to your services in an IOC container allows you to set up your game objects so that they're working against the interfaces of your services, rather than directly referencing the current implementation. This cleans up your service location quite a lot, and keeps your objects less tightly coupled to one another - you can replace the whole input service class used in your game by putting a new input service object in the IOC, rather than changes the references in all your objects.

It also means you don't have to fall back on the Singleton pattern so much in your code, such as setting a monolithic `InputService` class, with a static `GetInstance()` method.

This will make your code easier to test, because you can substitute your services with fakes when running integration testing. It also makes your code easier to maintain for the reasons above (with the added bonus that you don't need to add singletons to your code, which *also* will make your code easier to maintain and test).

Before you ask, even though the IOC uses a static reference method, the IOC itself is *not* a static class. The reference is actually a facade for the instantiated IOC components that you need to include in your scene. I chose to do this because the performance gains of using a service locator like this are rendered mute if we need to call `Find` each time we use it.

## Multiple IOC Containers, Persistent IOC Containers

You can include more than one IOC Container in your scene, and add different services to each. Why do this? Because IOC Containers can also be set as *persistent*, which means they won't be destroyed between scenes.

If it suits your architecture, you could set up a single IOC that holds all services uses throughout the lifetime of your game, and then include other, sub IOC Containers for specific scenes.

This isn't a heavily tested feature, so please use it with caution. If you do notice bugs and figure out how to resolve them please submit a bug report or (better yet) a pull request with a fix.

## Contributing

This is a very small project and I don't expect any outside contribution. However if you do use Fetch and you find a bug, please report it. If you fix the bug, please submit a PR together with an explanation of the bug. I would appreciate if fixes were sent with tests, but I understand that writing tests for this kind of thing is kind of difficult.

## Changes in v1.2

- I removed the Registry Service component. This makes the package more focused, and easier to maintain.

- I renamed the package from Fletch to Fetch. Fetch makes more sense, and nobody got the joke anyway.

- I cleaned up the tests a lot, and removed dependencies on some custom Test Helper classes I'd written. This makes it easier for people to contribute if they want to.

- I make the IOC more robust, and a little more flexible. It now adds any interface to the directory, irrespective of its name. The important part is that it's a child of the IOC container component.

- I added the persistent object functionality.
