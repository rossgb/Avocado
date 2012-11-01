﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avocado
{
	class SpatialHash
	{
		#region Fields

		Dictionary<Tuple<int, int>, List<Entity>> map;
		int cellSize;

		#endregion

		#region Initialization

		public SpatialHash(int cellSize)
		{
			this.cellSize = cellSize;
			this.map = new Dictionary<Tuple<int, int>, List<Entity>>();
		}

		#endregion

		#region Public methods

		public void Clear()
		{
			this.map.Clear();
		}

		public void Insert(Entity entity)
		{
			int a = (int) entity.Position.X / this.cellSize;
			int b = (int) entity.Position.Y / this.cellSize;
			Tuple<int, int> key = new Tuple<int,int>(a, b);

			if (!this.map.ContainsKey(key))
			{
				this.map[key] = new List<Entity>();
			}

			this.map[key].Add(entity);
		}

		public void Repopulate(List<Entity> entities)
		{
			this.Clear();
			entities.ForEach(entity => this.Insert(entity));
		}

		public List<Entity> Query(Entity entity)
		{
			List<Entity> collisionCandidates = new List<Entity>();
			List<Entity> candidatesInCell;

			int offset = Math.Max(1, entity.Radius / this.cellSize);
			int x = (int) entity.Position.X / this.cellSize;
			int y = (int) entity.Position.Y / this.cellSize;

			for (int i = x - offset; i <= x + offset; i++)
			{
				for (int j = y - offset; j <= y + offset; j++)
				{
					if (this.map.TryGetValue(new Tuple<int,int>(i, j), out candidatesInCell))
					{
						collisionCandidates.AddRange(candidatesInCell);
					}
				}
			}

			return collisionCandidates.Count > 0 ? collisionCandidates : null;
		}

		#endregion

		#region Class methods

		public static SpatialHash FromEntities(List<Entity> entities, int cellSize)
		{
			SpatialHash spatialHash = new SpatialHash(cellSize);
			entities.ForEach(entity => spatialHash.Insert(entity));

			return spatialHash;
		}

		#endregion
	}
}
