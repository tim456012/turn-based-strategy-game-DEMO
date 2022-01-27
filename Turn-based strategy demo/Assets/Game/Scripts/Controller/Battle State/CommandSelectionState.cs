using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectionState : BaseAbilityMenuState
{
    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Commands";
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Actions");
            menuOptions.Add("Wait");
        }
        
        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
    }
    
    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0:
                owner.ChangeState<MoveTargetState>();
                break;
            case 1:
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2:
                owner.ChangeState<SelectUnitState>();
                break;
        }
    }
    
    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            abilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.tile.pos);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }
}
