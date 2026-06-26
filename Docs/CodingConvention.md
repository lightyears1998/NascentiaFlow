# Coding Convention

## Limitations

- 通常，对于 Avalonia Control，参考[文档](https://docs.avaloniaui.net/docs/custom-controls/control-trees)，data binding 在进入视觉树时建立，在离开视觉树时清理。
  但对于一些控件的某些 Property（例如，DataGrid 的 ItemSelected 属性），由于其实现，在脱离视觉树后可能仍然会与 ViewModel 保持双向绑定。
  如果需要，应该避免使用 Avalonia data binding，可以使用 ReactiveUI data binding。 
