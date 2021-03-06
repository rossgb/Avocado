﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Avocado
{
	class SpatialHash<V> where V : Entity
	{
		#region Fields

		Dictionary<Vector2, List<V>> map;
		int cellSize;

		#endregion

		#region Initialization

		public SpatialHash(int cellSize)
		{
			this.cellSize = cellSize;
			this.map = new Dictionary<Vector2, List<V>>();
		}

		#endregion

		#region Public methods

		public void Clear()
		{ 
			this.map.Clear();
		}

		public void Insert(V entity)
		{
			int a = (int) entity.Position.X / this.cellSize;
			int b = (int) entity.Position.Y / this.cellSize;
			Vector2 key = new Vector2(a, b);

			if (!this.map.ContainsKey(key))
			{
				this.map[key] = new List<V>();
			}

			this.map[key].Add(entity);
		}

		public void Repopulate(List<V> entities)
		{
			this.Clear();
			entities.ForEach(entity => this.Insert(entity));
		}

		public List<V> Query(Entity entity)
		{
			List<V> collisionCandidates = new List<V>();
			List<V> candidatesInCell;
			Vector2 key = new Vector2();

			int offset = Math.Max(1, entity.Radius / this.cellSize);
			int x = (int) entity.Position.X / this.cellSize;
			int y = (int) entity.Position.Y / this.cellSize;

			for (int i = x - offset; i <= x + offset; i++)
			{
				for (int j = y - offset; j <= y + offset; j++)
				{
					key.X = i;
					key.Y = j;

					if (this.map.TryGetValue(key, out candidatesInCell))
					{
						collisionCandidates.AddRange(candidatesInCell);
					}
				}
			}

			var list = from candidate in collisionCandidates 
					   where Vector2.Distance(candidate.Position, entity.Position) <= (candidate.Radius + entity.Radius) 
					   select candidate;

			return list.ToList<V>();
		}

		#endregion
	}
}
