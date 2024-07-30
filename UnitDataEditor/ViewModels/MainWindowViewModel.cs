using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ReactiveUI;
using UnitDataEditor.UnitData;

namespace UnitDataEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private static FilePickerFileType UnitDataBin { get; } = new("Unit Data Binary")
    {
        Patterns = new[] { "*.bin" }
    };
    
    private IUnitData? _unitData;
    private EditorViewModel? _editorView;

    public EditorViewModel? EditorView
    {
        get => _editorView;
        set => this.RaiseAndSetIfChanged(ref _editorView, value);
    }
    
    public MainWindowViewModel()
    {
        _editorView = new DefaultViewModel();
    }

    public async void OpenFile()
    {
        if (Avalonia.Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        // Start async operation to open the dialog.
        var files = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Unit Data Binary (.bin)",
            AllowMultiple = false,
            FileTypeFilter = [ UnitDataBin ] 
        });

        if (files.Count < 1) return;
        // Open reading stream from the first file.
        await using var stream = await files[0].OpenReadAsync();
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var buffer = memoryStream.ToArray();

        switch (files[0].Name)
        {
            case "unit_Attack_Param.bin":
            {
                _unitData = new AttackParam();
                var success = _unitData.Parse(buffer);
                if (success != 0) _unitData = null;
                else
                {
                    EditorView = new AttackParamViewModel((_unitData as AttackParam)!);
                }
                break;
            }
            default:
            {
                break;
            }
        }
    }
    
    public async void SaveFile()
    {
        if (_unitData is null) return;
        if (EditorView is null) return;
        if (Avalonia.Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        EditorView.PrepareSave();
        
        // Start async operation to open the dialog.
        var file = await desktop.MainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Unit Data Binary (.bin)",
            DefaultExtension = ".bin",
            FileTypeChoices = [ UnitDataBin ],
        });

        if (file is null) return;
        // Open writing stream from the file.
        await using var stream = await file.OpenWriteAsync();
        await using var binaryWriter = new BinaryWriter(stream);
        binaryWriter.Write(_unitData.Rebuild());
    }
}