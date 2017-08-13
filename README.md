# Fetch

Fetch is a very light weight IOC Container for Unity. It allows you decouple your components in Unity, or at least make them not so *tightly* coupled.

## About

Fetch was designed to facilitate a specific Unity coding style that I've been using lately, so it's not really out there to implement everything you might want to see in an IOC Container. If you'll like an IOC project for Unity that is much more full featured and mature, then I suggest you check out [StrangeIoC](http://strangeioc.github.io/strangeioc/), which does way more things than Fetch sets out to achieve (though, I should note I haven't actually *used* this project, and it looks like it hasn't been updated for a while). On the other hand, if StrangeIoC seems to do way too much, and all you need is a system for fetching your service components by interface, then Fetch might be right for you.

Fetch allows you to store services in a number of ways, and access them relatively painlessly from inside your objects. It also has a fairly early stage object generation system, so you can use it to create new instances of services or object without having to use the `new` command, with automatic dependency serrching if you need that. 

## Making Services Available To Fetch

There are three ways that you can make a service available for retrieval or creation via Fetch:

### Access MonoBehaviours Via Service Container Children

A Service Container is a MonoBehaviour object that will, when queried, make a list of all the child objects that are available to it. Anything you add as a child object to the service container is accessible via the IOC.

The Service Container registers child objects via their interface type, rather than their specific types. That means that rather than MonoBehaviours doing a `Find()` on specific object types, you resolve to the interface, and the IOC feeds you whatever concrete class satisfies that interface.

For example:

```
ServiceContainer
 |- TouchInputMonoBehaviour: <IInputService>
 |- AudioMonoBehaviour: <IAudioService>
 |- ScoreMonoBehaviour: <IScoreService>
```

In the above arrangement, you would get an instance of the `TouchInputMonoBehaviour` object by asking the IOC container to resolve `IInputService`.

These objects can be retreived using `IOC.Retreive<T>()`. For example to fetch the`TouchInputMonoBehaviour` above, you would:

```csharp
IOC.Retreive<IInputService>();
```

## Access Non MonoBehaviour Services Via Service Container Proxies

In addition to putting MonoBehaviours into a Service Container, you can also use a MonoBehaviour as a bridge to another object via a Proxy. You do this by making your MonoBehaviour implement the `Fetch.IProxy` interface. Any object that implements `IProxy<T>` must provide a method `GetProxy` that returns an instance of `<T>`.

For example:

```
ServiceContainer
|- ScoreManagerMonoBheaviour: <IScoreManager> --> proxy for non-monobehaviour ScoreManager class
```

This might be implemented as follows:

```csharp
public interface IScoreManager () {
    void AddPoints(int points);
}

public class ScoreManager : IScoreManager () {
    private int points = 0;

    public void AddPoints(int points)
    {
        points += points;
    }
}
```

We want `ScoreManager` accessible from the IOC Container, but since it does not derive from MonoBehaviour, we can't add it as a child of the IOC Service. Instead of adding the methods directly to a MonoBehaviour object, we can create a proxy class:

```csharp
class ScoreManagerProxy : MonoBehaviour, Fetch.IProxy<IScoreManager> {
    
    private scoreManager;

    void Awake() 
    {
        scoreManager = new ScoreManager();
    }

    public IScoreManager GetProxy()
    {
        if (scoreManager == null) {
            scoreManager = new ScoreManager();
        }

        return scoreManager;
    }
}
```

This allows you to make game services without them having to be MonoBehaviours, which in turn will make your code easier to test.

Objects stored via proxy are fetched like anything else in the Service Container. For example to fetch the proxied ScoreManager class, you would:

```csharp
IOC.Resolve<IFooService>();
```

### Make New Instance Using Service Provider Binding

It is possible to directly bind resolution classes to the IOC using a `ServiceProvider` class. To do this, extend from the base `ServiceProvider` class, and add a Populate() method. Inside the Populate method, use the `Bind` and `Singleton` methods to make new class mappings available to the IOC container. The ServiceProvider class extends from MonoBehaviour, so you can add it to your scenes to make the bindings available on a per-scene basis.

Mappings can be defined using `Bind` or `Singleton`, and are accessible using the `IOC.Make<T>` method.

For example:

```csharp
class MyServiceProvider : Fetch.ServiceProvider 
{
    void Populate ()
    {
        // a new one of these will be provided each time
        Bind<IInputService, TouchInputService>();
        Bind<IAudioService, StreamingAudioService>();
        
        // only one of these will ever be created
        Singleton<IScoreManager, ScoreManager>();
    }
}
```

To make a the `TouchInputService` above, you would:

```csharp
IOC.Make<IInputService>();
```

Note that anything bound with the singleton behaviour will only be made the first time Make is called. Subsequent calls to make will return the previously made instance.

## Dependency Injection

Any call to `IOC.Make()` will try and build any dependencies that are present in the constructor of the object being resolved to.

For example, in the following case:

```csharp
public interface IScoreManager {
    void AddPoints(int points);
}

public class ScoreManager : IScoreManager {
    
    private IRenderer renderer;
    private int points = 0;

    public ScoreManager(IRenderer renderer) {
        this.renderer = renderer;
    }

    public void AddPoints(int points)
    {
        points += points;
        renderer.RenderPoints(points);
    }
}
```

The ScoreManager object is dependent on another class that implements `IRenderer`. If `IRenderer` is registered inside a service provider or service container, then any calls to `IOC.Make<IScoreManager>()` will pass the registered instance to the constructor.

**NOTE: Fetch will recursively find dependencies for generated classes, but will _not_ check to see if you have circular dependencies.**

You can also pass your own parameters to the `IOC.Make()` method, and these will be given priority when looking for dependencies to pass to the constructor. For example:

```csharp
public interface IGoblin 
{
    public string GetName();

    public int GetHealth();

    public void Damage(int damage);
}

public class Goblin : IGoblin
{
    string name;
    int health;
    IAudio audio;

    public Goblin(string name, int health, IAudio audio)
    {
        this.name = name;
        this.health = health;
        this.audio = audio;
    }

    public int GetHealth()
    {
        return health;
    }

    public string GetName()
    {
        return name;
    }

    public string Damage(int damage)
    {
        health -= damage;
        audio.PlaySound("goblin_scream.wav");
    }
}
```

Given the above class and interface, just calling `IOC.Make()` will trigger an error. To create a new instance of the Goblin class you would:

```csharp
IOC.Make<IGoblin>("fuge", 100);
```

The first two parameters would be passed along appropriately, and the last (missing) parameter will be filled in by Fetch. If two or more parameters share the same type, then Fetch will feed them to the constructor in the order they are received by `Make`.

### Make Concrete Classes

It is possible to use Fetch to satisfy the dependency on concrete classes that are not registered in the service provider. Fetch will look at the constructor for the class and try find matching dependencies at run time. This means that the above class could also be resolved as:

```csharp
IOC.Make<Goblin>("puga", 50);
```

There is also an alternative version of this method, `IOC.Make<T,R>()` which will return the object of type `T` cast as type `R`.

## Inject Bindings (Testing) 

** This Feature is still in prototype **

It's possible to inject a binding and its resolution at run time. This will make Fetch always return the specified instance whenever a call to `Make` is received with the matching type. This is done using the `IOC.InjectBinding<T>(T)` method. For example:

```csharp
var audio = Substitute.For<IAudio>();

IOC.InjectBinding<IAudio>(audio);

var resolved = IOC.Make<IAudio>();
```

Both `resolved` and `audio` would point to the same object.

Using `InjectBinding` will currently ignore any parameters that are passed to the IOC outside of the type.

This feature is intended for use testing classes that have calls to the IOC in hard to reach places.

## Why should you an IOC Container?

Putting references to your services in an IOC container allows you to set up your game objects so that they're working against the interfaces of your services, rather than directly referencing the current implementation. This cleans up your service location quite a lot, and keeps your objects less tightly coupled to one another - you can replace the whole input service class used in your game by putting a new input service object in the IOC, or binding a new one in your service provider, rather than changing the references in all your objects.

It also means you don't have to fall back on the Singleton pattern so much in your code. You don't need to set a monolithic `InputService` class, with a static `GetInstance()` method. Instead, you can use a service provider with a Singleton mapping, or you could create a single service, and make it the child of a service container.

Using an IOC Container to provide your services and objects via interfaces will also make your code *much easier to test*.

## Multiple Service Containers, Persistent Service Containers

You can include more than one Service Container in your scene, and add different services to each. 

Why do this? IOC Containers can also be set as *persistent*, which means they won't be destroyed between scenes. If it suits your architecture, you could set up a single IOC that holds all services uses throughout the lifetime of your game, and then include other, sub IOC Containers for specific scenes.

**This isn't a heavily tested feature, so please use it with caution. If you do notice bugs and figure out how to resolve them please submit a bug report or (better yet) a pull request with a fix.**

## Multiple Service Providers

You can also include multiple service providers, but they aren't persistent between scenes. You will need to include them in each scene where the services are required.

## Contributing

This is a very small project and I don't expect any outside contribution. However if you do use Fetch and you find a bug, please report it. If you fix the bug, please submit a PR together with an explanation of the bug. I would appreciate if fixes were sent with tests, but I understand that writing tests for this kind of thing is kind of difficult. I have included as many tests as I could together with the source (this requires the Unity Test Tools to run).

## Installing

There is a package in the `Packages` folder that you can import into your Unity projects. This will install Fetch without the tests. If you want to work on improving Fetch itself, then I suggest checking out the repository.

Once this project feels a little more mature (read: the public interface doesn't change so often), I'll see about submitting a version to the asset store for easier installation.
