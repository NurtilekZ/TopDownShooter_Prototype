namespace _current.Core.Systems.AbilitySystem
{
    public class AbilityBuilder
    {
        private AbilityConfig _config;
        protected IAbility _ability;

        public AbilityBuilder(AbilityConfig config)
        {
            _config = config;
        }

        public virtual void Build()
        {
            if (_ability != null)
            {
                _ability = new Ability(
                    _config.Title,
                    _config.Description,
                    _config.Image,
                    _config.CooldownTime,
                    _config.Cost,
                    _config.KeyBind);
            }
        }

        public virtual IAbility GetResult() => _ability;
    }
}