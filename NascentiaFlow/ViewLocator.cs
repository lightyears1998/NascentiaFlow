using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NascentiaFlow.ViewModels;
using NascentiaFlow.Views;

namespace NascentiaFlow;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}

public class SceneLocator : IDataTemplate
{
    public virtual Control? GetStubControl()
    {
        return new StubScene();
    }

    public virtual Type? GetCorrespondingControlType(SceneModelBase? vm)
    {
        if (vm is null)
            return null;

        // NascentiaFlow.ViewModels.FooBarSceneModel -> NascentiaFlow.Views.FooBarScene
        var name = vm.GetType().FullName!
            .Replace("SceneModel", "Scene", StringComparison.Ordinal)
            .Replace(".ViewModels", ".Views", StringComparison.Ordinal);
        return Type.GetType(name);
    }
    
    public Control? Build(object? data)
    {
        if (data is not SceneModelBase vm)
            return new StubScene();

        var type = GetCorrespondingControlType(vm);
        if (type == null)
            return GetStubControl();

        var control = (Control)Activator.CreateInstance(type)!;
        control.DataContext = vm;
        return control;
    }

    public bool Match(object? data)
    {
        return data is SceneModelBase;
    }
}

public class SceneToolPanelLocator : SceneLocator
{
    public override Control? GetStubControl()
    {
        return new StubSceneToolPanel();
    }

    public override Type? GetCorrespondingControlType(SceneModelBase? vm)
    {
        if (vm is null)
            return null;

        // NascentiaFlow.ViewModels.FooBarSceneModel -> NascentiaFlow.Views.FooBarSceneToolPanel
        var name = vm.GetType().FullName!
            .Replace("SceneModel", "SceneToolPanel", StringComparison.Ordinal)
            .Replace(".ViewModels", ".Views", StringComparison.Ordinal);
        return Type.GetType(name);
    }
}
