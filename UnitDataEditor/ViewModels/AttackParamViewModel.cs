using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using UnitDataEditor.UnitData;

namespace UnitDataEditor.ViewModels;

public class AttackParamViewModel : EditorViewModel
{
    private readonly AttackParam _attackParamMem;
    public ObservableCollection<AttackData> AttackDatas { get; }
    private AttackData _currentAttackData;
    public AttackData CurrentAttackData
    {
        get => _currentAttackData;
        set => this.RaiseAndSetIfChanged(ref _currentAttackData, value);
    }
    
    public void AddMove()
    {
        CurrentAttackData = new AttackData();
        AttackDatas.Add(CurrentAttackData);
    }
    
    public void DeleteMove()
    {
        AttackDatas.Remove(CurrentAttackData);
        if (AttackDatas.Count != 0)
        {
            CurrentAttackData = AttackDatas[0];
        }
        else
        {
            CurrentAttackData = new AttackData();
            AttackDatas.Add(CurrentAttackData);
        }
    }

    public AttackParamViewModel(AttackParam inAttackParam)
    {
        _attackParamMem = inAttackParam;
        AttackDatas = _attackParamMem.AttackDatas != null ? new ObservableCollection<AttackData>(_attackParamMem.AttackDatas) : [];
        if (AttackDatas.Count != 0)
        {
            CurrentAttackData = AttackDatas[0];
        }
        else
        {
            CurrentAttackData = new AttackData();
            AttackDatas.Add(CurrentAttackData);
        }
    }

    public AttackParamViewModel()
    {
        _attackParamMem = new AttackParam();
        AttackDatas = [];
        CurrentAttackData = new AttackData();
        AttackDatas.Add(CurrentAttackData);
    }
    public override void PrepareSave()
    {
        _attackParamMem.AttackDatas = AttackDatas.ToList();
    }
}