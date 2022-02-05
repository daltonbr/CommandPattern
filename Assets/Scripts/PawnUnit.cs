using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnUnit : Unit
{
    public int Distance;
    
    public override int GetMoveCells(Vector3Int[] result, Gameboard board)
    {
        int count = 0;
        
        for (int y = -Distance; y <= Distance; ++y)
        {
            for (int x = -Distance; x <= Distance; ++x)
            {
                if(x == 0 && y == 0)
                    continue;
                
                if(Mathf.Abs(x) + Mathf.Abs(y) > Distance)
                    continue;
                
                var idx = m_CurrentCell + new Vector3Int(x,0, y);
                if (board.IsOnBoard(idx))
                {
                    result[count] = idx;
                    count++;
                }
            }
        }

        return count;
    }
}
