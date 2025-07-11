using Godot;
using System;
using System.Collections;

public partial class CharacterStat : Node
{
    public enum AffectStamina
    {
        Rest, Nap, Sleep, Walk, Run
    }
    public enum ActionType
    {
        Attacked, Heal
    }
    public enum Operation
    {
        Add, Substract, Multiply
    }
    //For each class inherit this class, set a default value
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Stamina { get; private set; }
    public float MaxStamina { get; private set; }


    public override void _Ready()
    {
        base._Ready();
    }
    private void LoadSave(string saveDataName)
    {
        //load from save file the stat here
        //Create a new dictionary, save to it
    }
    public void ModHealth(float modifier, ActionType action)
    {
        switch (action)
        {
            case ActionType.Attacked:
                CurrentHealth -= modifier;
                break;
            case ActionType.Heal:
                CurrentHealth += modifier;
                break;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
        checkDeath();
    }
    public void ModMaxHealth(float modifier, Operation operation)
    {
        //this is to increase max possible health, which is achieved via items, level upgrade, etc.
        switch (operation)
        {
            case Operation.Multiply:
                MaxHealth *= modifier;
                break;
            case Operation.Add:
                MaxHealth += modifier;
                break;
            case Operation.Substract:
                MaxHealth -= modifier;
                break;
        }
    }
    private void checkDeath()
    {
        if (CurrentHealth <= 0f)
        {
            //trigger change of state
            GD.Print("Noel perished");
        }
    }
    private void resetHealth() => CurrentHealth = MaxHealth;
    private void resetStamina() => Stamina = MaxStamina;
}
