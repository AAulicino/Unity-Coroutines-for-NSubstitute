Unity Coroutines for [NSubstitute](https://nsubstitute.github.io/)
========

## What is it?

Testing and mocking Unity Coroutines can be tricky. This is an extension for
[NSubstitute](https://nsubstitute.github.io/) that is designed to help you at mocking Unity
Coroutines.

## Basic use

Lets use this simple counter class using coroutines for testing:

```csharp
    public class Counter
    {
        public int Current { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine coroutine;

        public Counter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void StartCounter ()
        {
            coroutine = runner.StartCoroutine(CounterRoutine());
        }

        public void StopCounter ()
        {
            runner.StopCoroutine(coroutine);
            coroutine = null;
        }

        IEnumerator CounterRoutine ()
        {
            while (true)
            {
                Current++;
                yield return null;
            }
        }
    }
```

One thing you might've noticed is that instead of directly referencing a MonoBehaviour to call
StartCoroutine, we're using an interface for the runner. This allows us to mock the runner in our
tests.

### Creating the mock

To mock the ICoroutineRunner we need to create a mock for it. A simplified way to do so is by
calling:

```
ICoroutineRunner runner = CoroutineSubstitute.Create();
```

To preserve the Syntax provided by NSubstitute, this alternate version can be done instead

```
ICoroutineRunner runner = Substitute.ForPartsOf<CoroutineRunnerSubstitute>();
```

### Using the mock

The Counter can now be tested as follows:

```csharp
    // Arrange
    ICoroutineRunner runner = CoroutineSubstitute.Create();
    Counter counter = new Counter(Runner);
    // Act
    Counter.Start();
    Runner.MoveNext();
    // Assert
    Assert.AreEqual(1, Counter.Current);
```

Calling Runner.MoveNext() will simulate Unity's coroutine update loop.

You can check the [CounterTests.cs](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/blob/main/Tests/Editor/Samples/Counter/CounterTests.cs) for test examples on the [Counter](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/blob/main/Tests/Editor/Samples/Counter/Counter.cs) class.

Since MonoBehaviours implement all methods specified in the ICoroutineRunner interface, you can
simply add it to your MonoBehaviour, for example:

```csharp
public class MyCoroutineRunner : MonoBehaviour, ICoroutineRunner
{
}
```

```csharp
public class GameSetup : MonoBehaviour
{
    [SerializeField] public MyCoroutineRunner coroutineRunner;
    Counter counter;

    void Start ()
    {
        counter = new Counter(coroutineRunner);
        counter.Start();
    }
}
```

Other samples can be found in the [Samples](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/tree/main/Tests/Editor/Samples) folder.
