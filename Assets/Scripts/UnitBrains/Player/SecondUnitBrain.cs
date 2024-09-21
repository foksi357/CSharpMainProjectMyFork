using System;
using System.Collections.Generic;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {       
            float overheatTemperature = OverheatTemperature;

            if (GetTemperature() >= overheatTemperature)
            {
                return;
            }

            for (int i = 0; i < GetTemperature() + 1f; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }

            IncreaseTemperature();



        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        {

            List<Vector2Int> result = GetReachableTargets();
            if (result.Count > 1) { 
            //Создаем переменную для присвоение ей значения
            Vector2Int Target = result[0];
            //Создаем переменную для вычесления минимального значения
            float Mindistance = int.MaxValue;
            //Перебираем список 
            foreach (Vector2Int target1 in result)
            {
                //С помощью метода узнаем расстояние противника до нашей базы и вычисляем
                if (DistanceToOwnBase(target1) < Mindistance)
                {
                    //Возращаем в метод полученное значение
                    Mindistance = DistanceToOwnBase(target1);
                    //Возращаем значение в список
                    Target = target1;
                }
            }
            //Очищаем список
            result.Clear();
            //Добавляем в список полученное значение
            result.Add(Target);

        } 
            return result;
            
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}