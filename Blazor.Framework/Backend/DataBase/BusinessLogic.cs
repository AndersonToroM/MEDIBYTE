namespace Dominus.Backend.DataBase
{
    public class BusinessLogic
    {
        #region Properties

        public DataBaseSetting settings;

        #endregion

        #region CTor


        public BusinessLogic(DataBaseSetting settings)
        {
            this.settings = settings;
        }

        #endregion
    }
}
