using System;

namespace imbSCI.Core.style.preset
{
    using imbSCI.Core.extensions.enumworks;

    /// <summary>
    /// Preset item tuple
    /// </summary>
    public class propertyAnnotationPresetItemTuple
    {
        public propertyAnnotationPresetItemTuple()
        {
        }

        public propertyAnnotationPresetItemTuple(String _key, String _value)
        {
            key = _key;

            value = _value;
        }

        private Object _resolvedKey;

        /// <summary> Key in proper enum type</summary>
        public Object resolvedKey
        {
            get
            {
                if (_resolvedKey == null)
                {
                    _resolvedKey = ResolveKey();
                }
                return _resolvedKey;
            }
        }

        internal Object ResolveKey()
        {
            Object _key = null;
            if (key.Contains("."))
            {
                _key = imbEnumExtendBase.getEnumMemberByPath(key, propertyAnnotationPreset.supportedEnumTypes);
            }
            else
            {
                _key = (String)key;
            }

            if (_key == null)
            {
                if (key.Contains("."))
                {
                    throw new Exception("propertyAnnotationPresetItemTuple.key [" + key + "] type, not recognized as supported enum type");
                }
                else
                {
                }
            }

            return _key;
        }

        public String key { get; set; } = "";

        public String value { get; set; } = null;
    }
}