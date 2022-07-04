Unity Coroutines for NSubstitute
========
[![Tests](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/actions/workflows/main.yml/badge.svg)](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/actions/workflows/main.yml)
[![openupm](https://img.shields.io/npm/v/com.aaulicino.unity-coroutines-for-nsubstitute?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.aaulicino.unity-coroutines-for-nsubstitute/)
- [Unity Coroutines for NSubstitute](#unity-coroutines-for-nsubstitute)
  * [What is it?](#what-is-it)
  * [Installation](#installation)
  * [Basic use](#basic-use)
    + [Creating the mock](#creating-the-mock)
    + [Using the mock](#using-the-mock)
    + [Custom Assertions](#custom-assertions)

## What is it?

Testing and mocking Unity Coroutines can be tricky. This is an extension for
[NSubstitute](https://nsubstitute.github.io/) that is designed to help you at mocking Unity
Coroutines.

## Installation

### OpenUPM
You can install this package using [OpenUPM](https://openupm.com/packages/com.aaulicino.unity-coroutines-for-nsubstitute). 

### Manual Installation
Unity does not allow specifying a git URL as a dependency of a custom UPM Package.

If you don't have NSubstitute already from another source, add the following to your **manifest.json**:

```json
"com.aaulicino.nsubstitute": "https://github.com/AAulicino/Unity3D-NSubstitute.git"
```

After ensuring you have NSubstitue installed, then place this in your **manifest.json**:

```json
"com.aaulicino.unity-coroutines-for-nsubstitute": "https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute.git"
```

## Basic use

### Creating the mock

You can mock the ICoroutineRunner interface by calling:

```csharp
ICoroutineRunner runner = CoroutineSubstitute.Create();
```

To preserve the Syntax provided by NSubstitute, this alternate version can be used instead:

```csharp
ICoroutineRunner runner = Substitute.ForPartsOf<CoroutineRunnerSubstitute>();
```

### Using the mock

Let's use this simple counter class for testing:

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

    public void Start ()
    {
        coroutine = runner.StartCoroutine(CounterRoutine());
    }

    public void Stop ()
    {
        runner.StopCoroutine(coroutine);
        coroutine = null;
    }

    IEnumerator CounterRoutine ()
    {
        while (true)
        {
            Current++;
            yield return new WaitForSeconds(1);
        }
    }
}
```

One thing you might've noticed is that instead of calling StartCoroutine on a MonoBehaviour,
we're calling it on the ICoroutineRunner interface. This allows us to mock the runner in our tests.

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

Calling `Runner.MoveNext()` will simulate Unity's coroutine update loop.

You can check the [CounterTests.cs](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/blob/main/Tests/Editor/Samples/Counter/CounterTests.cs)
for test examples on the [Counter](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/blob/main/Tests/Editor/Samples/Counter/Counter.cs) class.

Since MonoBehaviours implement all methods specified in the ICoroutineRunner interface, you can
simply add it to your MonoBehaviour, for example:

```csharp
public class GameSetup : MonoBehaviour, ICoroutineRunner
{
    Counter counter;

    void Start ()
    {
        counter = new Counter(this);
        counter.Start();
    }
}
```

### Custom Assertions

You can also assert the yielded values from the coroutine:

```csharp
ICoroutineRunner runner = CoroutineSubstitute.Create();
Counter counter = new Counter(Runner);

Runner.MoveNextAndExpect<WaitForSeconds>();
```

To assert the amount of seconds configured in the `WaitForSeconds` object:
```csharp
ICoroutineRunner runner = CoroutineSubstitute.Create();
Counter counter = new Counter(Runner);

Runner.MoveNextAndExpect(new WaitForSeconds(1));
```


Other samples can be found in the [Samples](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/tree/main/Tests/Editor/Samples) folder.

---

You can also have a look at the [SystemTests](https://github.com/AAulicino/Unity-Coroutines-for-NSubstitute/tree/main/Tests/Editor/SystemTests) folder as the tests found in there represent some real uses cases.
