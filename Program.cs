using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace OpenTK.EventDeliveryTest;

enum EventType
{
    Event1,
    Event2,
    Event3,
    Event4,
    Event5,
    Event6,
    Event7,
    Event8,
    Event9,
    Event10,
    Event11,
    Event12,
    Event13,
    Event14,
    Event15,
    Event16,
    EventMax,
}

abstract class MyEventArgs(EventType type) : EventArgs
{
    public EventType Type { get; } = type;
    public abstract EventType TypeVirtual { get; }

    public static MyEventArgs GetRandom(Random random)
    {
        float dummy = random.NextSingle();
        switch ((EventType)random.Next((int)EventType.EventMax))
        {
            case EventType.Event1: return new MyEventArgs1(dummy);
            case EventType.Event2: return new MyEventArgs2(dummy);
            case EventType.Event3: return new MyEventArgs3(dummy);
            case EventType.Event4: return new MyEventArgs4(dummy);
            case EventType.Event5: return new MyEventArgs5(dummy);
            case EventType.Event6: return new MyEventArgs6(dummy);
            case EventType.Event7: return new MyEventArgs7(dummy);
            case EventType.Event8: return new MyEventArgs8(dummy);
            case EventType.Event9: return new MyEventArgs9(dummy);
            case EventType.Event10: return new MyEventArgs10(dummy);
            case EventType.Event11: return new MyEventArgs11(dummy);
            case EventType.Event12: return new MyEventArgs12(dummy);
            case EventType.Event13: return new MyEventArgs13(dummy);
            case EventType.Event14: return new MyEventArgs14(dummy);
            case EventType.Event15: return new MyEventArgs15(dummy);
            case EventType.Event16: return new MyEventArgs16(dummy);
        }

        return null!;
    }
}

// Dummy here is a dummy workload to sum together.

class MyEventArgs1(float dummy) : MyEventArgs(EventType.Event1)
{
    public override EventType TypeVirtual => EventType.Event1;
    public float Dummy { get; } = dummy;
}
class MyEventArgs2(float dummy) : MyEventArgs(EventType.Event2)
{
    public override EventType TypeVirtual => EventType.Event2;
    public float Dummy { get; } = dummy;
}

class MyEventArgs3(float dummy) : MyEventArgs(EventType.Event3)
{
    public override EventType TypeVirtual => EventType.Event3;
    public float Dummy { get; } = dummy;
}

class MyEventArgs4(float dummy) : MyEventArgs(EventType.Event4)
{
    public override EventType TypeVirtual => EventType.Event4;
    public float Dummy { get; } = dummy;
}

class MyEventArgs5(float dummy) : MyEventArgs(EventType.Event5)
{
    public override EventType TypeVirtual => EventType.Event5;
    public float Dummy { get; } = dummy;
}

class MyEventArgs6(float dummy) : MyEventArgs(EventType.Event6)
{
    public override EventType TypeVirtual => EventType.Event6;
    public float Dummy { get; } = dummy;
}

class MyEventArgs7(float dummy) : MyEventArgs(EventType.Event7)
{
    public override EventType TypeVirtual => EventType.Event7;
    public float Dummy { get; } = dummy;
}

class MyEventArgs8(float dummy) : MyEventArgs(EventType.Event8)
{
    public override EventType TypeVirtual => EventType.Event8;
    public float Dummy { get; } = dummy;
}

class MyEventArgs9(float dummy) : MyEventArgs(EventType.Event9)
{
    public override EventType TypeVirtual => EventType.Event9;
    public float Dummy { get; } = dummy;
}

class MyEventArgs10(float dummy) : MyEventArgs(EventType.Event10)
{
    public override EventType TypeVirtual => EventType.Event10;
    public float Dummy { get; } = dummy;
}

class MyEventArgs11(float dummy) : MyEventArgs(EventType.Event11)
{
    public override EventType TypeVirtual => EventType.Event11;
    public float Dummy { get; } = dummy;
}

class MyEventArgs12(float dummy) : MyEventArgs(EventType.Event12)
{
    public override EventType TypeVirtual => EventType.Event12;
    public float Dummy { get; } = dummy;
}

class MyEventArgs13(float dummy) : MyEventArgs(EventType.Event13)
{
    public override EventType TypeVirtual => EventType.Event13;
    public float Dummy { get; } = dummy;
}

class MyEventArgs14(float dummy) : MyEventArgs(EventType.Event14)
{
    public override EventType TypeVirtual => EventType.Event14;
    public float Dummy { get; } = dummy;
}

class MyEventArgs15(float dummy) : MyEventArgs(EventType.Event15)
{
    public override EventType TypeVirtual => EventType.Event15;
    public float Dummy { get; } = dummy;
}

class MyEventArgs16(float dummy) : MyEventArgs(EventType.Event16)
{
    public override EventType TypeVirtual => EventType.Event16;
    public float Dummy { get; } = dummy;
}


[MemoryDiagnoser]
public class Benchmark
{
    const int NumberOfEventsToGenerate = 32 * (1 << 20);

    private readonly List<MyEventArgs> Events = new List<MyEventArgs>();
    private event EventHandler<MyEventArgs>? OnEvent;

    public Benchmark()
    {
        Random random = new Random();

        for (int i = 0; i < NumberOfEventsToGenerate; i++)
        {
            Events.Add(MyEventArgs.GetRandom(random));
        }
    }

    public void DispatchEvents()
    {
        foreach (MyEventArgs args in Events)
        {
            OnEvent?.Invoke(this, args);
        }
    }

    [Benchmark(Baseline = true)]
    public void MatchOnEnum()
    {
        float sum = 0f;

        OnEvent += (sender, args) =>
        {
            sum += args.Type switch
            {
                EventType.Event1 => ((MyEventArgs1)args).Dummy,
                EventType.Event2 => ((MyEventArgs2)args).Dummy,
                EventType.Event3 => ((MyEventArgs3)args).Dummy,
                EventType.Event4 => ((MyEventArgs4)args).Dummy,
                EventType.Event5 => ((MyEventArgs5)args).Dummy,
                EventType.Event6 => ((MyEventArgs6)args).Dummy,
                EventType.Event7 => ((MyEventArgs7)args).Dummy,
                EventType.Event8 => ((MyEventArgs8)args).Dummy,
                EventType.Event9 => ((MyEventArgs9)args).Dummy,
                EventType.Event10 => ((MyEventArgs10)args).Dummy,
                EventType.Event11 => ((MyEventArgs11)args).Dummy,
                EventType.Event12 => ((MyEventArgs12)args).Dummy,
                EventType.Event13 => ((MyEventArgs13)args).Dummy,
                EventType.Event14 => ((MyEventArgs14)args).Dummy,
                EventType.Event15 => ((MyEventArgs15)args).Dummy,
                EventType.Event16 => ((MyEventArgs16)args).Dummy,
                _ => 0,
            };
        };

        DispatchEvents();

        Console.WriteLine($"Total = {sum}");

        OnEvent = null;
    }

    [Benchmark]
    public void MatchOnEnumVirtual()
    {
        float sum = 0f;

        OnEvent += (sender, args) =>
        {
            sum += args.TypeVirtual switch
            {
                EventType.Event1 => ((MyEventArgs1)args).Dummy,
                EventType.Event2 => ((MyEventArgs2)args).Dummy,
                EventType.Event3 => ((MyEventArgs3)args).Dummy,
                EventType.Event4 => ((MyEventArgs4)args).Dummy,
                EventType.Event5 => ((MyEventArgs5)args).Dummy,
                EventType.Event6 => ((MyEventArgs6)args).Dummy,
                EventType.Event7 => ((MyEventArgs7)args).Dummy,
                EventType.Event8 => ((MyEventArgs8)args).Dummy,
                EventType.Event9 => ((MyEventArgs9)args).Dummy,
                EventType.Event10 => ((MyEventArgs10)args).Dummy,
                EventType.Event11 => ((MyEventArgs11)args).Dummy,
                EventType.Event12 => ((MyEventArgs12)args).Dummy,
                EventType.Event13 => ((MyEventArgs13)args).Dummy,
                EventType.Event14 => ((MyEventArgs14)args).Dummy,
                EventType.Event15 => ((MyEventArgs15)args).Dummy,
                EventType.Event16 => ((MyEventArgs16)args).Dummy,
                _ => 0,
            };
        };

        DispatchEvents();

        Console.WriteLine($"Total = {sum}");

        OnEvent = null;
    }

    [Benchmark]
    public void MatchOnType()
    {
        float sum = 0f;

        OnEvent += (sender, args) =>
        {
            sum += args switch
            {
                MyEventArgs1  ea => ea.Dummy,
                MyEventArgs2  ea => ea.Dummy,
                MyEventArgs3  ea => ea.Dummy,
                MyEventArgs4  ea => ea.Dummy,
                MyEventArgs5  ea => ea.Dummy,
                MyEventArgs6  ea => ea.Dummy,
                MyEventArgs7  ea => ea.Dummy,
                MyEventArgs8  ea => ea.Dummy,
                MyEventArgs9  ea => ea.Dummy,
                MyEventArgs10 ea => ea.Dummy,
                MyEventArgs11 ea => ea.Dummy,
                MyEventArgs12 ea => ea.Dummy,
                MyEventArgs13 ea => ea.Dummy,
                MyEventArgs14 ea => ea.Dummy,
                MyEventArgs15 ea => ea.Dummy,
                MyEventArgs16 ea => ea.Dummy,
                _ => 0,
            };
        };

        DispatchEvents();

        Console.WriteLine($"Total = {sum}");

        OnEvent = null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}
