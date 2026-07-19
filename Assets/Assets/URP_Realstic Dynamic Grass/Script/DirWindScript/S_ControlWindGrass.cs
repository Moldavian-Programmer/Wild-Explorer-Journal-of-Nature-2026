using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class S_ControlWindGrass : MonoBehaviour
{
    
    public Material[] m_GrassMats;
    public DIRECTION_WIND m_DirectionWind;
    public S_GrassWindDirectionData m_DirData;
    private Sequence m_DirSequence;
    private void Start()
    {
        Func_SetWindGrass();
    }
    public void Func_SetWindGrass()
    {
        //Use DOTween to change windDirection Slowly
        if (m_GrassMats.Length <= 0) return;
        Vector2 _CurrentDirGrass = m_GrassMats[0].GetVector("_WindDir");
        Vector2 _NextDir = new Vector2();
        if (m_DirSequence.IsActive()) m_DirSequence.Kill();
        m_DirSequence = DOTween.Sequence();
        float _ChangeDirTime = 1;
        switch (m_DirectionWind)
        {
            case DIRECTION_WIND.NORTH:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_North._X, m_DirData.m_North._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.SOUTH:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_South._X, m_DirData.m_South._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.EAST:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_East._X, m_DirData.m_East._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.WEST:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_West._X, m_DirData.m_West._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.NORTHEAST:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_NorthEast._X, m_DirData.m_NorthEast._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.NORTHWEST:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_NorthWest._X, m_DirData.m_NorthWest._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.SOUTHEAST:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_NorthEast._X, m_DirData.m_SouthEast._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
            case DIRECTION_WIND.SOUTHWEST:
                //Set Direc for all
                _NextDir = new Vector2(m_DirData.m_SouthWest._X, m_DirData.m_SouthWest._Y);
                m_DirSequence.Append(DOTween.To(() => _CurrentDirGrass, x => _CurrentDirGrass = x, _NextDir, _ChangeDirTime).OnUpdate(() =>
                {
                    for (int i = 0; i < m_GrassMats.Length; i++)
                    {
                        m_GrassMats[i].SetVector("_WindDir", new Vector4(_CurrentDirGrass.x, _CurrentDirGrass.y, 0, 0));
                    }
                }));
                break;
        }
    
    }
    public void Func_SetDir(int _DirIndex)
    {
        //Change m_DirectionWind , use by UI Button
        switch (_DirIndex)
        {
            case 0:
                m_DirectionWind = DIRECTION_WIND.NORTH;
                break;
            case 1:
                m_DirectionWind = DIRECTION_WIND.SOUTH;
                break;
            case 2:
                m_DirectionWind = DIRECTION_WIND.EAST;
                break;
            case 3:
                m_DirectionWind = DIRECTION_WIND.WEST;
                break;
            case 4:
                m_DirectionWind = DIRECTION_WIND.NORTHEAST;
                break;
            case 5:
                m_DirectionWind = DIRECTION_WIND.NORTHWEST;
                break;
            case 6:
                m_DirectionWind = DIRECTION_WIND.SOUTHEAST;
                break;
            case 7:
                m_DirectionWind = DIRECTION_WIND.SOUTHWEST;
                break;
        }
        Func_SetWindGrass();
    }


}
