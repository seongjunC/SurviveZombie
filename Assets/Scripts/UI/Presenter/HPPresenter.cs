using System;
using Monster;
using Player;
using UI;
using UnityEngine;

public class HPPresenter
{
    private HealthBar _view;
    private PlayerController _player;
    private MonsterController _monster;

    private int maxHp;

    public HPPresenter(HealthBar view, PlayerController player)
    {
        _view = view;
        _player = player;
    }

    public HPPresenter(HealthBar view, MonsterController monster)
    {
        _view = view;
        _monster = monster;
    }

    public void Initialize()
    {
        if (_monster is null)  PlayerInit();
        else if (_player is null) MonsterInit();
    }

    private void PlayerInit()
    {
        maxHp = (int)_player.GetStatus(PlayerStatusType.maxHealth);
        _player.SubscribeEvent(PlayerStatusType.currentHealth, UpdateView);
        
        UpdateView((int)_player.GetStatus(PlayerStatusType.currentHealth));
    }

    private void MonsterInit()
    {
        maxHp = _monster.maxHp;
        _monster.OnHealthChanged += UpdateView;
        
        UpdateView(_monster.currentHp);
    }

    public void UpdateView(int hp)
    {
        _view.SetHealth(hp, maxHp);
    }

    public void Dispose()
    {
        if (_player is not null) _player.UnsubscribeEvent(PlayerStatusType.currentHealth, UpdateView);
        else if (_monster is not null) _monster.OnHealthChanged -= UpdateView;
    }
    
}