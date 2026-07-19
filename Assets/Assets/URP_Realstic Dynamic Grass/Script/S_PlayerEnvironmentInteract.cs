using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class S_PlayerEnvironmentInteract : MonoBehaviour
{
    public Transform[] m_MoveToPos;
    int m_CurentMovetoPosIndex = 0;
    private void Start()
    {
        Func_MoveTo();
    }
    public void Func_MoveTo()
    {
        if (m_MoveToPos.Length <= 0) return;
        transform.DOMove(m_MoveToPos[m_CurentMovetoPosIndex].position, 20).OnComplete(() =>
        {
            if (m_CurentMovetoPosIndex == m_MoveToPos.Length - 1)
            {
                m_CurentMovetoPosIndex = 0;
            }
            else
                m_CurentMovetoPosIndex += 1;
            Func_MoveTo();
        });
    }
    private void OnTriggerEnter(Collider other)
    {
        //Grass
        if(other.CompareTag("Grass"))
        {
            
            if(other.GetComponent<S_InteractGrassLOD>())
            {
                other.GetComponent<S_InteractGrassLOD>().Func_TrigerEnterWithPlayer(transform.position);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Grass
        if (other.CompareTag("Grass"))
        {
            if(other.GetComponent<S_InteractGrassLOD>())
            {
                other.GetComponent<S_InteractGrassLOD>().Func_TriggerStayWithPlayer(transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Grass
        if (other.CompareTag("Grass"))
        {
            if (other.GetComponent<S_InteractGrassLOD>())
            {
                other.GetComponent<S_InteractGrassLOD>().Func_OnTriggerExitWithPlayer();
            }
        }
    }
}
