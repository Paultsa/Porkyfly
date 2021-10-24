using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Obstacle))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Obstacle script = (Obstacle)target;

        //script.player = (GameObject)EditorGUILayout.ObjectField("Player", script.player, typeof(GameObject), true);
        script.backgroundManager = (GameObject)EditorGUILayout.ObjectField("Background manager", script.backgroundManager, typeof(GameObject), true);
        script.speed = EditorGUILayout.FloatField("Speed of the obstacle", script.speed);
        script.minHeight = EditorGUILayout.FloatField("Minimun height for spawning", script.minHeight);
        script.maxHeight = EditorGUILayout.FloatField("Maximum height for spawning", script.maxHeight);

        script.animal = (Obstacle.animalType)EditorGUILayout.EnumPopup("Animal type", script.animal);

        EditorGUILayout.LabelField("What type of obstacle");
        script.obstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Obstacle type", script.obstacleType);

        //Primary
        if (script.obstacleType == Obstacle.obstacleTypes.Bounce)
        {
            script.velocityMultiplierPercent = EditorGUILayout.FloatField("Velocity percent", script.velocityMultiplierPercent);
            script.velocityFixedBoost = EditorGUILayout.FloatField("Fixed boost", script.velocityFixedBoost);
            script.yToXPercentageIncrease = EditorGUILayout.FloatField("Y to X (%) increase", script.yToXPercentageIncrease);
            EditorGUILayout.HelpBox("How many percent faster does the player move after colliding with this object", MessageType.None);
            script.secondaryObstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Seondary type", script.secondaryObstacleType);
        }
        if (script.obstacleType == Obstacle.obstacleTypes.Slow)
        {
            script.velocitySlowPercent = EditorGUILayout.FloatField("Slow percent", script.velocitySlowPercent);
            EditorGUILayout.HelpBox("How many percent slower does the player move after colliding with this object. Max value is 100", MessageType.None);
            script.secondaryObstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Seondary type", script.secondaryObstacleType);
        }
        if (script.obstacleType == Obstacle.obstacleTypes.BoostPickup)
        {
            script.boostAmount = EditorGUILayout.IntField("Boosts", script.boostAmount);
            EditorGUILayout.HelpBox("How boosts are given back to the player", MessageType.None);
            script.secondaryObstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Seondary type", script.secondaryObstacleType);
        }
        if (script.obstacleType == Obstacle.obstacleTypes.BoostDisable)
        {
            script.boostDisableTime = EditorGUILayout.FloatField("Disable duration", script.boostDisableTime);
            EditorGUILayout.HelpBox("How long is the player's boost disabled for", MessageType.None);
            script.secondaryObstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Seondary type", script.secondaryObstacleType);
        }
        if (script.obstacleType == Obstacle.obstacleTypes.AirDragThorns)
        {
            script.speedLossPercentPerSecond = EditorGUILayout.FloatField("Speed loss per second in percent", script.speedLossPercentPerSecond);
            script.airDragThornsDuration = EditorGUILayout.FloatField("Condition duration", script.airDragThornsDuration);
            EditorGUILayout.HelpBox("This obstacle gives the player drag, slowing the player in air for the duration", MessageType.None);
            script.secondaryObstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Seondary type", script.secondaryObstacleType);
        }
        if (script.obstacleType == Obstacle.obstacleTypes.BearCannon)
        {
            script.cannonMinMultiplier = EditorGUILayout.FloatField("Minimum force percent", script.cannonMinMultiplier);
            script.cannonMaxMultiplier = EditorGUILayout.FloatField("Maximum force percent", script.cannonMaxMultiplier);
            script.cannonMaxValueHitBonus = EditorGUILayout.FloatField("Max hit bonus force", script.cannonMaxValueHitBonus);
            script.cannonMinFixedBonus = EditorGUILayout.FloatField("Minimum force fixed", script.cannonMinFixedBonus);
            script.cannonMaxFixedBonus = EditorGUILayout.FloatField("Maximum force fixed", script.cannonMaxFixedBonus);
            script.cannonMaxValueHitFixedBonus = EditorGUILayout.FloatField("Max hit bonus fixed force", script.cannonMaxValueHitFixedBonus);
            script.cannonDuration = EditorGUILayout.FloatField("Aim duration", script.cannonDuration);
            script.cannonArrow = (GameObject)EditorGUILayout.ObjectField("Direction arrow prefab", script.cannonArrow, typeof(GameObject), false);
            script.powerBar = (GameObject)EditorGUILayout.ObjectField("Power meter prefab", script.powerBar, typeof(GameObject), false);
            script.throwablePos = (GameObject)EditorGUILayout.ObjectField("Throwable wait pos", script.throwablePos, typeof(GameObject), true);
            EditorGUILayout.HelpBox("This obstacle grabs the player, gives a chance to redirect the player and multiplies the force. Minimum force is good to be under 100", MessageType.None);
            script.secondaryObstacleType = Obstacle.obstacleTypes.None;
        }
        if (script.obstacleType == Obstacle.obstacleTypes.HawkGrab)
        {
            script.hawkFlightAngle = EditorGUILayout.FloatField("Angle of flight", script.hawkFlightAngle);
            script.hawkAccelerationPercentage = EditorGUILayout.FloatField("acceleration %/sec", script.hawkAccelerationPercentage);
            script.hawkAccelerationFixed = EditorGUILayout.FloatField("acceleration units/sec", script.hawkAccelerationFixed);
            script.hawkDuration = EditorGUILayout.FloatField("Powerup duration", script.hawkDuration);
            //script.throwablePos = (GameObject)EditorGUILayout.ObjectField("Throwable wait pos", script.throwablePos, typeof(GameObject), true);
            EditorGUILayout.HelpBox("This obstacle grabs the player and starts accelerating at given angle for the duration, then releases the player at the end", MessageType.None);
            script.secondaryObstacleType = Obstacle.obstacleTypes.None;
        }
        if (script.obstacleType == Obstacle.obstacleTypes.CashSquirrel)
        {
            script.minCash = EditorGUILayout.IntField("Minimum cash earned", script.minCash);
            script.maxCash = EditorGUILayout.IntField("Maximum cash earned", script.maxCash);
            script.cashExplosion = (GameObject)EditorGUILayout.ObjectField("Cash explosion prefab", script.cashExplosion, typeof(GameObject), false);
            EditorGUILayout.HelpBox("This obstacle gives the player some cash when hit", MessageType.None);
            script.secondaryObstacleType = (Obstacle.obstacleTypes)EditorGUILayout.EnumPopup("Seondary type", script.secondaryObstacleType);
            //script.secondaryObstacleType = Obstacle.obstacleTypes.None;
        }


        //Secondary
        if (script.secondaryObstacleType == Obstacle.obstacleTypes.Bounce)
        {
            script.velocityMultiplierPercent = EditorGUILayout.FloatField("Velocity percent", script.velocityMultiplierPercent);
            script.velocityFixedBoost = EditorGUILayout.FloatField("Fixed boost", script.velocityFixedBoost);
            script.yToXPercentageIncrease = EditorGUILayout.FloatField("Y to X (%) increase", script.yToXPercentageIncrease);
            EditorGUILayout.HelpBox("How many percent faster does the player move after colliding with this object", MessageType.None);
        }
        if (script.secondaryObstacleType == Obstacle.obstacleTypes.Slow)
        {
            script.velocitySlowPercent = EditorGUILayout.FloatField("Slow percent", script.velocitySlowPercent);
            EditorGUILayout.HelpBox("How many percent slower does the player move after colliding with this object. Max value is 100", MessageType.None);
        }
        if (script.secondaryObstacleType == Obstacle.obstacleTypes.BoostPickup)
        {
            script.boostAmount = EditorGUILayout.IntField("Boosts", script.boostAmount);
            EditorGUILayout.HelpBox("How boosts are given back to the player", MessageType.None);
        }
        if (script.secondaryObstacleType == Obstacle.obstacleTypes.BoostDisable)
        {
            script.boostDisableTime = EditorGUILayout.FloatField("Disable duration", script.boostDisableTime);
            EditorGUILayout.HelpBox("How long is the player's boost disabled for", MessageType.None);
        }
        if (script.secondaryObstacleType == Obstacle.obstacleTypes.AirDragThorns)
        {
            script.speedLossPercentPerSecond = EditorGUILayout.FloatField("Speed loss per second in percent", script.speedLossPercentPerSecond);
            script.airDragThornsDuration = EditorGUILayout.FloatField("Condition duration", script.airDragThornsDuration);
            EditorGUILayout.HelpBox("This obstacle gives the player drag, slowing the player in air for the duration", MessageType.None);
        }

    }
}
