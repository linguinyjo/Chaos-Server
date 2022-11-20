namespace Chaos.Scripting.Abstractions;

/// <summary>
///     Represents a <see cref="IScript"/> intended to be used for a specific object
/// </summary>
/// <typeparam name="T">The <see cref="IScripted"/> object this script is attached to</typeparam>
public abstract class SubjectiveScriptBase<T> : ScriptBase where T: IScripted
{
    public T Subject { get; }

    protected SubjectiveScriptBase(T subject) => Subject = subject;
}