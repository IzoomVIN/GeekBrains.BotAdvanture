using UnityEngine;

namespace Pool
{
    public class ObjectPool
    {
        private Container _elementFlag;

        public GameObject GetElement()
        {
            if (_elementFlag == null) return null;

            var el = _elementFlag.element;
            _elementFlag = _elementFlag.next;

            return el;
        }

        public void SetElement(GameObject el)
        {
            _elementFlag = new Container(_elementFlag, el);
        }
    
        private class Container
        {
            public readonly Container next;
            public readonly GameObject element;

            public Container(Container next, GameObject element)
            {
                this.next = next;
                this.element = element;
            }
        }
    }
}
