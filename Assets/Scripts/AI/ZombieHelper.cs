using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ZombieHelper
{
    private static float movementAnimationDelay = 0.3f,spitballTravelTime=0.9f;

    public static float MovementAnimationDelay { get => movementAnimationDelay; }
    public static float SpitballTravelTime { get => spitballTravelTime; }

    public static IEnumerator EndTurnAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameEvents.instance.UnitDoneMoving();
    }
    /// <summary>
    /// returns a list of all heroes directly adjacent to position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static List<Unit> HeroesInMeleeRange(Vector2Int position)
    {
        List<Unit> heroesInMeleeRange = new List<Unit>();

        Vector2Int adjacentSpace = new Vector2Int(position.x + 1, position.y);
        if (MapContent.instance.SpaceContainsHero(adjacentSpace))
        {
            heroesInMeleeRange.Add(MapContent.instance.Dictionary[adjacentSpace]);
        }
        adjacentSpace = new Vector2Int(position.x - 1, position.y);
        if (MapContent.instance.SpaceContainsHero(adjacentSpace))
        {
            heroesInMeleeRange.Add(MapContent.instance.Dictionary[adjacentSpace]);
        }
        adjacentSpace = new Vector2Int(position.x, position.y + 1);
        if (MapContent.instance.SpaceContainsHero(adjacentSpace))
        {
            heroesInMeleeRange.Add(MapContent.instance.Dictionary[adjacentSpace]);
        }
        adjacentSpace = new Vector2Int(position.x, position.y - 1);
        if (MapContent.instance.SpaceContainsHero(adjacentSpace))
        {
            heroesInMeleeRange.Add(MapContent.instance.Dictionary[adjacentSpace]);
        }

        return heroesInMeleeRange;
    }
    /// <summary>
    /// returns a list of all heroes inside of range measured from startPosition
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static List<Unit> HeroesInRange(Vector2Int startPosition, int range)
    {
        List<Unit> heroesInRange = new List<Unit>();
        List<Vector2Int> spacesInRange = GetSpacesInRange(startPosition, range);
        foreach (Vector2Int spaceInRange in spacesInRange)
        {
            if (MapContent.instance.SpaceContainsHero(spaceInRange))
            {
                heroesInRange.Add(MapContent.instance.Dictionary[spaceInRange]);
            }
        }
        return heroesInRange;
    }
    /// <summary>
    /// returns a list of all PathfindingNodes leading towards tiles which are adjacent to a hero.
    /// </summary>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    public static List<PathfindingNode> GetPathsInMeleeRange(Vector2Int startPosition)
    {
        List<PathfindingNode> pathsInMeleeRange = new List<PathfindingNode>();

        //check each living hero
        foreach (Unit currentHero in HeroManager.instance.AllHeroes)
        {
            if (currentHero == null || currentHero.Health <= 0)
            {
                continue;
            }
            Vector2Int[] adjacentSpaces = new Vector2Int[4];
            adjacentSpaces[0] = new Vector2Int(currentHero.gridPosition.x + 1, currentHero.gridPosition.y);
            adjacentSpaces[1] = new Vector2Int(currentHero.gridPosition.x - 1, currentHero.gridPosition.y);
            adjacentSpaces[2] = new Vector2Int(currentHero.gridPosition.x, currentHero.gridPosition.y + 1);
            adjacentSpaces[3] = new Vector2Int(currentHero.gridPosition.x, currentHero.gridPosition.y - 1);

            //see if adjacent spaces are reachable from startPosition
            foreach (Vector2Int adjacentSpace in adjacentSpaces)
            {
                if (IsoGrid.instance.IsInsideBounds(adjacentSpace))
                {
                    if (!MapContent.instance.Dictionary.ContainsKey(adjacentSpace))
                    {
                        PathfindingNode possiblePath = Pathfinding.FindPath(startPosition, adjacentSpace);
                        if (possiblePath != null)
                        {
                            //add reachable tiles which are adjacent to a hero to the list
                            pathsInMeleeRange.Add(possiblePath);
                        }
                    }
                }
            }

        }

        return pathsInMeleeRange;
    }
    /// <summary>
    /// returns a List containing a PathfindingNode for each tile which is free and in range of a hero. 
    /// range is determined by zombie-attackRange
    /// </summary>
    /// <param name="zombie"></param>
    /// <returns></returns>
    public static List<PathfindingNode> GetPathsInRange(Unit zombie)
    {
        List<PathfindingNode> pathsInRange = new List<PathfindingNode>();


        //searchedSpaces are added to a HashSet to make sure they are only checked once
        HashSet<Vector2Int> searchedSpaces = new HashSet<Vector2Int>();

        //check each living hero
        foreach (Unit currentHero in HeroManager.instance.AllHeroes)
        {
            if (currentHero == null || currentHero.Health <= 0)
            {
                continue;
            }

            List<Vector2Int> possibleAttackSpaces = ZombieHelper.GetFreeSpacesInRange(currentHero.gridPosition, zombie.AttackRange);
            //see if tiles in range of zombies attack-range are reachable from startPosition
            foreach (Vector2Int possibleAttackSpace in possibleAttackSpaces)
            {
                if (!searchedSpaces.Contains(possibleAttackSpace))
                {
                    searchedSpaces.Add(possibleAttackSpace);

                    PathfindingNode possiblePath = Pathfinding.FindPath(zombie.gridPosition, possibleAttackSpace);
                    if (possiblePath != null)
                    {
                        //add reachable tiles which are adjacent to a hero to the list
                        pathsInRange.Add(possiblePath);
                    }
                }
                
            }
        }
        return pathsInRange;
    }
    /// <summary>
    /// Get all unoccupied spaces in range of a given position
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static List<Vector2Int> GetFreeSpacesInRange(Vector2Int startPosition, int range)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        //add startPosition to list
        positions.Add(startPosition);
        for (int i = 0; i < range; i++)
        {
            Vector2Int[] positionsArray = positions.ToArray();

            //check all adjacent spaces of tiles inside positions and add them if they are free
            foreach (Vector2Int position in positionsArray)
            {
                Vector2Int adjacentSpace = new Vector2Int(position.x + 1, position.y);
                if (MapContent.instance.SpaceIsFree(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
                adjacentSpace = new Vector2Int(position.x - 1, position.y);
                if (MapContent.instance.SpaceIsFree(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
                adjacentSpace = new Vector2Int(position.x, position.y + 1);
                if (MapContent.instance.SpaceIsFree(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
                adjacentSpace = new Vector2Int(position.x, position.y - 1);
                if (MapContent.instance.SpaceIsFree(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
            }
        }
        return positions;
    }
    /// <summary>
    /// Get all spaces in range of a given position
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static List<Vector2Int> GetSpacesInRange(Vector2Int startPosition, int range)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        //add startPosition to list
        positions.Add(startPosition);
        for (int i = 0; i < range; i++)
        {
            Vector2Int[] positionsArray = positions.ToArray();
            //check all adjacent spaces of tiles inside positions and add them
            foreach (Vector2Int position in positionsArray)
            {
                Vector2Int adjacentSpace = new Vector2Int(position.x + 1, position.y);
                if (IsoGrid.instance.IsInsideBounds(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
                adjacentSpace = new Vector2Int(position.x - 1, position.y);
                if (IsoGrid.instance.IsInsideBounds(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
                adjacentSpace = new Vector2Int(position.x, position.y + 1);
                if (IsoGrid.instance.IsInsideBounds(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
                adjacentSpace = new Vector2Int(position.x, position.y - 1);
                if (IsoGrid.instance.IsInsideBounds(adjacentSpace) && !positions.Contains(adjacentSpace))
                {
                    positions.Add(adjacentSpace);
                }
            }
        }
        return positions;
    }
    /// <summary>
    /// choose the unit with the least health from a list of units
    /// </summary>
    /// <param name="heroesInRange"></param>
    /// <returns></returns>
    public static Unit GetBestTarget(List<Unit> heroesInRange)
    {
        Unit bestHeroToAttack = heroesInRange[0];
        for (int i = 1; i < heroesInRange.Count; i++)
        {
            if (heroesInRange[i].Health < bestHeroToAttack.Health)
            {
                bestHeroToAttack = heroesInRange[i];
            }
        }
        return bestHeroToAttack;
    }
    /// <summary>
    /// choose the shortest path from a list of paths
    /// </summary>
    /// <param name="allPaths"></param>
    /// <returns></returns>
    public static PathfindingNode GetShortestPath(List<PathfindingNode> allPaths)
    {
        PathfindingNode bestPath = allPaths[0];
        foreach (PathfindingNode currentPath in allPaths)
        {
            if (currentPath.stepsToPathtaker < bestPath.stepsToPathtaker)
            {
                bestPath = currentPath;
            }
        }
        return bestPath;
    }
    /// <summary>
    /// Animate walk animation towards a destination then animate attack animation, deal damage to heroes and end turn
    /// </summary>
    /// <param name="eachStepToDestination"></param>
    /// <param name="walker"></param>
    /// <returns></returns>
    public static IEnumerator AnimateWalkThenMeleeAttack(List<PathfindingNode> eachStepToDestination, Unit walker)
    {
        //start walk-animation
        if (walker.MyAnimator != null)
        {
            walker.MyAnimator.SetBool("walking", true);
        }
        //move unit along path with a short delay between each step
        foreach (PathfindingNode current in eachStepToDestination)
        {
            if (current.Position == eachStepToDestination[eachStepToDestination.Count-1].Position)
            {
                walker.MoveUnitTo(current.Position);
                break;
            }
            walker.MoveUnitTo(current.Position);

            yield return new WaitForSeconds(movementAnimationDelay);
        }
        //stop walk-animation
        if (walker.MyAnimator != null)
        {
            walker.MyAnimator.SetBool("walking", false);
        }
 

        //start attack
        List<Unit> heroesInRange = ZombieHelper.HeroesInMeleeRange(walker.gridPosition);

        if (heroesInRange.Count > 0)
        {
            Unit bestHeroToAttack = GetBestTarget(heroesInRange);

            //start attack-animation
            if (walker.MyAnimator != null)
            {
                walker.MyAnimator.SetBool("attacking", true);
            }
            yield return new WaitForSeconds(walker.AttackSoundEffectDelay);
            //play attack-soundeffect
            if (walker.AttackAudio != null)
            {
                walker.AttackAudio.Play();
            }
            bestHeroToAttack.TakeDamage(walker.GetAttackDamage());

            yield return new WaitForSeconds(walker.AttackDuration - walker.AttackSoundEffectDelay);
            //stop attack-animation
            if (walker.MyAnimator != null)
            {
                walker.MyAnimator.SetBool("attacking", false);
            }
        }
        GameEvents.instance.UnitDoneMoving();
    }
    /// <summary>
    /// Animate the melee attack of a zombie and deal damage to best adjacent hero and end turn
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns></returns>
    public static IEnumerator AnimateMeleeAttack( Unit attacker)
    {
        List<Unit> heroesInRange = ZombieHelper.HeroesInMeleeRange(attacker.gridPosition);

        if (heroesInRange.Count > 0)
        {
            Unit bestHeroToAttack = GetBestTarget(heroesInRange);

            //start attack-animation
            if (attacker.MyAnimator != null)
            {
                attacker.MyAnimator.SetBool("attacking", true);
            }
            yield return new WaitForSeconds(attacker.AttackSoundEffectDelay);
            
            //play attack-soundeffect
            if (attacker.AttackAudio != null)
            {
                attacker.AttackAudio.Play();
            }
            bestHeroToAttack.TakeDamage(attacker.GetAttackDamage());

            yield return new WaitForSeconds(attacker.AttackDuration - attacker.AttackSoundEffectDelay);
            //stop attack-animation
            if (attacker.MyAnimator != null)
            {
                attacker.MyAnimator.SetBool("attacking", false);
            }


        }
        GameEvents.instance.UnitDoneMoving();
    }
    /// <summary>
    /// Animate walk along given path then animate ranged attack of a zombie and deal damage to best hero in range and end turn
    /// </summary>
    /// <param name="eachStepToDestination"></param>
    /// <param name="walker"></param>
    /// <param name="spitUpPrefab"></param>
    /// <param name="spitDownPrefab"></param>
    /// <returns></returns>
    public static IEnumerator AnimateWalkThenRangedAttack(List<PathfindingNode> eachStepToDestination, Unit walker,GameObject spitUpPrefab,GameObject spitDownPrefab)
    {
        //start walk-animation
        if (walker.MyAnimator != null)
        {
            walker.MyAnimator.SetBool("walking", true);
        }
        //move unit along path with a short delay between each step
        foreach (PathfindingNode current in eachStepToDestination)
        {
            if (current.Position == eachStepToDestination[eachStepToDestination.Count - 1].Position)
            {
                walker.MoveUnitTo(current.Position);
                break;
            }
            walker.MoveUnitTo(current.Position);

            yield return new WaitForSeconds(movementAnimationDelay);
        }
        //stop walk-animation
        if (walker.MyAnimator != null)
        {
            walker.MyAnimator.SetBool("walking", false);
        }

        //start attack
        List<Unit> heroesInRange = ZombieHelper.HeroesInRange(walker.gridPosition,walker.AttackRange);

        if (heroesInRange.Count > 0)
        {
            //start attack-animation
            if (walker.MyAnimator != null)
                walker.MyAnimator.SetBool("attacking", true);
            yield return new WaitForSeconds(walker.AttackSoundEffectDelay);

            //play attack-soundeffect
            if (walker.AttackAudio != null)
                walker.AttackAudio.Play();

            //spawn a spitball shooting upwards at the mouth of this zombie
            GameObject.Instantiate(spitUpPrefab, walker.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(walker.AttackDuration - walker.AttackSoundEffectDelay);
            //stop attack-animation
            if (walker.MyAnimator != null)
            {
                walker.MyAnimator.SetBool("attacking", false);
            }
            Unit bestHeroToAttack = GetBestTarget(heroesInRange);

            //spawn a spitball shooting towards the target
            GameObject.Instantiate(spitDownPrefab, new Vector3(bestHeroToAttack.transform.position.x, bestHeroToAttack.transform.position.y+10), Quaternion.identity)
                .GetComponentInChildren<SpitBallAnimation>().ShootDownwardsTowards(IsoGrid.instance.ToWorldSpace(bestHeroToAttack.gridPosition)) ;

            yield return new WaitForSeconds(spitballTravelTime);

            //after the animation has played, target receives damage
            bestHeroToAttack.TakeDamage(walker.GetAttackDamage());
            GameEvents.instance.UnitDoneMoving();

        }
        else
        {
            //no heroes in range, end turn
            GameEvents.instance.UnitDoneMoving();
        }
    }
    /// <summary>
    /// Animate ranged attack of a zombie and deal damage to best hero in range
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="spitUpPrefab"></param>
    /// <param name="spitDownPrefab"></param>
    /// <returns></returns>
    public static IEnumerator AnimateRangedAttack( Unit attacker,GameObject spitUpPrefab,GameObject spitDownPrefab)
    {

        List<Unit> heroesInRange = ZombieHelper.HeroesInRange(attacker.gridPosition,attacker.AttackRange);

        if (heroesInRange.Count > 0)
        {
            if (attacker.MyAnimator != null)
            {
                attacker.MyAnimator.SetBool("attacking", true);
            }
            yield return new WaitForSeconds(attacker.AttackSoundEffectDelay);

            //play attack-soundeffect
            if (attacker.AttackAudio != null)
                attacker.AttackAudio.Play();

            //spawn a spitball shooting upwards at the mouth of this zombie
            GameObject.Instantiate(spitUpPrefab, attacker.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(attacker.AttackDuration - attacker.AttackSoundEffectDelay);
            //stop attack-animation
            if (attacker.MyAnimator != null)
            {
                attacker.MyAnimator.SetBool("attacking", false);
            }

            Unit bestHeroToAttack = GetBestTarget(heroesInRange);
            //spawn a spitball shooting towards the target
            GameObject.Instantiate(spitDownPrefab, new Vector3(bestHeroToAttack.transform.position.x, bestHeroToAttack.transform.position.y+10), Quaternion.identity)
                .GetComponentInChildren<SpitBallAnimation>().ShootDownwardsTowards(IsoGrid.instance.ToWorldSpace(bestHeroToAttack.gridPosition)) ;

            yield return new WaitForSeconds(spitballTravelTime);

            //after the animation has played, target receives damage
            bestHeroToAttack.TakeDamage(attacker.GetAttackDamage());
            GameEvents.instance.UnitDoneMoving();

        }
        else
        {
            //no heroes in range, end turn
            GameEvents.instance.UnitDoneMoving();
        }
    }
}
