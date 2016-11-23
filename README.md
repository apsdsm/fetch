# Fetch

Fetch is a very light weight IOC Container for Unity. It allows you decouple your components in Unity, or at least make them not so *tightly* coupled.

## About

Fetch was designed to facilitate a specific Unity coding style that I've been using lately, so it's not really out there to implement everything you might want to see in an IOC Container. If you'll like an IOC project for Unity that is much more full featured and mature, then I suggest you check out [StrangeIoC](http://strangeioc.github.io/strangeioc/), which does way more things than Fetch sets out to achieve.

On the other hand, if StrangeIoC seems to do way too much, and all you need is a system for fetching your service components by interface, then Fetch might be right for you.

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

## Using A Bridge

If you're trying to use tests in your game, you'll often find yourself creating MonoBehaviour objects whose sole purpose is to act as an intermediary between the game world and some other non-MonoBehaviour object (particularly managers, or services).

There are lots of ways of doing this - and most of them are pretty messy. Using a *bridge* is one way to simplify the process and formalize the pattern.

Say we have this class:

```csharp
public interface IFooService () {
    void DoFoo();
}

public class FooService () {
    public void DoFoo();
}
```

We want `FooService` accessible from the IOC Container, but since it does not derive from MonoBehaviour, we can't add it as a child of the IOC Service. Instead of adding FooService directly, we can create a bridge:

```csharp
class FooBridge : MonoBehaviour, Fetch.IBridge<IFooService> {
    
    private _service;

    void Awake() {
        _service = new FooService();
    }

    public IFooService bridged {
        get {
            return _service;
        }
    }
}
```

Now, if we add the bridge to the IOC container, we can resolve directly to `IFooService`. This allows us to keep the service itself as a 'pure' object, and leave any interaction between the object and the game itself inside the bridge class.

## Contributing

This is a very small project and I don't expect any outside contribution. However if you do use Fetch and you find a bug, please report it. If you fix the bug, please submit a PR together with an explanation of the bug. I would appreciate if fixes were sent with tests, but I understand that writing tests for this kind of thing is kind of difficult.