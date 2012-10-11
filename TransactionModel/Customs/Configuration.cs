using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;

namespace TransactionModel
{
    public partial class Configuration
    {
        #region Static Methods

        /// <summary>
        /// Return a config value given group and name
        /// </summary>
        /// <param name="configGroup">A config group</param>
        /// <param name="configName">A config name</param>
        /// <returns>The configuration or null if not found</returns>
        public static Configuration GetConfiguration(TransactionModelContainer container, string grp, string name)
        {
            Configuration val = null;

            var targets = from cfg in container.Configurations
                          where cfg.Group == grp && cfg.Name == name
                          select cfg;

            val = targets.FirstOrDefault();

            return val;
        }


        public static Configuration GetConfiguration(string group, string name)
        {
            using (var container = new TransactionModelContainer())
            {
                return container.Configurations.FirstOrDefault(x => x.Group == group && x.Name == name);
            }
        }



        /// <summary>
        /// Create a set of configurations
        /// *** SaveChanges is required to be called ***
        /// </summary>
        /// <param name="group">A group</param>
        /// <param name="names">Names, must be unique within the group</param>
        /// <param name="values1">A value, cannot be null</param>
        /// <param name="values2">A another value, can be null</param>
        /// <param name="values3">A the other value, can be null</param>
        /// <param name="descs">A description</param>
        /// <returns>Newly created configurations</returns>
        public static Configuration[] CreateConfigurationSet(TransactionModelContainer container, 
            string createBy, string group, string[] names,
            string[] values1, string[] values2, string[] values3, string[] descs)
        {
            if (string.IsNullOrEmpty(group))
            {
                throw new ArgumentException("NULL_OR_EMPTY_GROUP");
            }

            if (names == null || names.Length <= 0)
            {
                throw new ArgumentException("NULL_OR_EMPTY_NAMES");
            }

            if (values1 == null || values1.Length <= 0)
            {
                throw new ArgumentException("NULL_OR_EMPTY_VALUES1");
            }

            if (names.Length != values1.Length)
            {
                throw new ArgumentException("NUMBER_OF_NAMES_AND_VALUES1_NOT_MATCH");
            }

            Configuration[] configurations = new Configuration[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                configurations[i] = new Configuration(createBy, group, names[i], values1[i]);
                configurations[i].Value2 = values2 != null && values2.Length >= (i + 1) ? values2[i] : null;
                configurations[i].Value3 = values3 != null && values3.Length >= (i + 1) ? values3[i] : null;
                configurations[i].Description = descs != null && descs.Length >= (i + 1) ? descs[i] : null;
                container.Configurations.AddObject(configurations[i]);
            }

            return configurations;
        }

        /// <summary>
        /// Update a configuration
        /// </summary>
        /// <param name="_group">The configuration group</param>
        /// <param name="name">The configuration name</param>
        /// <param name="value1">The first value</param>
        /// <param name="value2">The second value (optional-set to null to clear)</param>
        /// <param name="value3">The third value (optional-set to null to clear)</param>
        /// <param name="desc">The description (optional-set to null to clear)</param>
        /// <returns>The updated configuration</returns>  
        /// <remarks>container.SaveChanges is required</remarks>
        public static Configuration UpdateConfiguration(TransactionModelContainer container, 
            string _group, string name,
            string value1, string value2, string value3, string desc)
        {
            Configuration config = GetConfiguration(container, _group, name);

            if (config == null)
            {
                throw new ApplicationException("CONFIG_NOT_FOUND");
            }

            config.Value1 = value1;
            config.Value2 = value2;
            config.Value3 = value3;
            config.Description = desc;

            return config;
        }

        /// <summary>
        /// Delete the configuration
        /// </summary>
        /// <param name="_group">The configuration group</param>
        /// <param name="name">The configuration name</param>
        /// <remarks>container.SaveChanges is required</remarks>
        public static Configuration DeleteConfiguration(TransactionModelContainer container, string _group, string name)
        {
            Configuration config = GetConfiguration(container, _group, name);

            if (config == null)
            {
                throw new ApplicationException("CONFIG_NOT_FOUND");
            }

            container.DeleteObject(config);

            return config;
        }        

        #endregion

        /// <summary>
        /// Clears configuration values
        /// *** SaveChanges is required ***
        /// </summary>
        /// <param name="updateBy">The one updating this configuration, required</param>
        /// <param name="clearValue2">True to clear Value2</param>
        /// <param name="clearValue3">True to clear Value3</param>
        /// <param name="clearDescription">True to clear Description</param>
        public void ClearValues(string updateBy, bool clearValue2, bool clearValue3, bool clearDescription)
        {
            if (string.IsNullOrEmpty(updateBy))
            {
                throw new ArgumentException("UPDATE_BY_IS_REQUIRED");
            }

            if (clearValue2)
            {
                Value2 = null;
            }

            if (clearValue3)
            {
                Value3 = null;
            }

            if (clearDescription)
            {
                Description = null;
            }

            if (clearValue2 || clearValue3 || clearDescription)
            {
                UpdateBy = updateBy;
                UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Updates config values
        /// *** SaveChanges is required ***
        /// </summary>
        /// <param name="updateBy">The one updating this configuration, required</param>
        /// <param name="value1">Value1, put null to ignore</param>
        /// <param name="value2">Value2, put null to ignore</param>
        /// <param name="value3">Value3, put null to ignore</param>
        /// <param name="description">Description, put null to ignore</param>
        public void UpdateValues(string updateBy, string value1, string value2, string value3, string description)
        {
            if (string.IsNullOrEmpty(updateBy))
            {
                throw new ArgumentException("UPDATE_BY_IS_REQUIRED");
            }

            bool changed = false;

            if (!string.IsNullOrEmpty(value1) && value1 != Value1)
            {
                Value1 = value1;
                changed = true;
            }

            if (!string.IsNullOrEmpty(value2) && value2 != Value2)
            {
                Value2 = value2;
                changed = true;
            }

            if (!string.IsNullOrEmpty(value3) && value3 != Value3)
            {
                Value3 = value3;
                changed = true;
            }

            if (!string.IsNullOrEmpty(description) && description != Description)
            {
                Description = description;
                changed = true;
            }

            if (changed)
            {
                UpdateBy = updateBy;
                UpdateDate = DateTime.Now;
            }
        }

        public Configuration(string createdBy, string group, string name,
            string value1, string value2, string value3, string desc)
        {
            Group = group;
            Name = name;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Description = desc;

            CreateBy = createdBy;
            CreateDate = DateTime.Now;
            UpdateBy = createdBy;
            UpdateDate = DateTime.Now;
        }

        public Configuration(string createdBy, string group, string name, string value1)
            : this(createdBy, group, name, value1, null, null, null)
        {

        }

        // Use by EF only!!!
        internal Configuration() { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().Name + "{");
            sb.AppendFormat("Group={0}, Name={1}, Value1={2}", Group, Name, Value1);
            sb.AppendLine("}");
            return sb.ToString();
        }

        #region Partial Methods

        partial void OnGroupChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_GROUP");
            }

            if (value.Length > 20)
            {
                throw new ArgumentException("GROUP_TOO_LONG");
            }
        }

        partial void OnNameChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_NAME");
            }

            if (value.Length > 20)
            {
                throw new ArgumentException("NAME_TOO_LONG");
            }
        }

        partial void OnValue1Changing(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_VALUE1");
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("VALUE1_TOO_LONG");
            }
        }

        partial void OnValue2Changing(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 100)
                {
                    throw new ArgumentException("VALUE2_TOO_LONG");
                }
            }
        }

        partial void OnValue3Changing(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 100)
                {
                    throw new ArgumentException("VALUE3_TOO_LONG");
                }
            }
        }

        partial void OnCreateByChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CREATE_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("CREATE_BY_TOO_LONG");
            }
        }

        partial void OnCreateDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("CREATE_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnUpdateByChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_UPDATE_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("UPDATE_BY_TOO_LONG");
            }
        }

        partial void OnUpdateDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("UPDATE_DATE_NOT_PERSISTABLE");
            }
        }

        #endregion
    }
}
