using Unity.Netcode.Components;
using UnityEngine;

namespace GB.Networks
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative() => false;
    }
}
