using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportLine : MonoBehaviour
{
    public ITransportLineStart start;
    public List<ITransportLineEnd> end;


}

public interface ITransportLineStart
{
    public bool TrySetEnd(ITransportLineEnd end);
}

public interface ITransportLineEnd
{
    
}
