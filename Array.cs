using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace RustMapEditor.Maths
{
	// Token: 0x020004F0 RID: 1264
	public static class Array
	{
		// Token: 0x0600295B RID: 10587 RVA: 0x000B5348 File Offset: 0x000B3548
		public static float[,] SetValues(float[,] array, float value, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					array[i, j] = value;
				}
			});
			return array;
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000B53B4 File Offset: 0x000B35B4
		public static float[,,] SetValues(float[,,] array, int channel, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			int channelLength = array.GetLength(2);
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					for (int k = 0; k < channelLength; k++)
					{
						array[i, j, k] = 0f;
					}
					array[i, j, channel] = 1f;
				}
			});
			return array;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000B5430 File Offset: 0x000B3630
		public static bool[,] SetValues(bool[,] array, bool value, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = new AreaManager.Area(AreaManager.ActiveArea.x0, AreaManager.ActiveArea.x1 * 2, AreaManager.ActiveArea.z0, AreaManager.ActiveArea.z1 * 2);
			}
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					array[i, j] = value;
				}
			});
			return array;
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000B54C8 File Offset: 0x000B36C8
		public static float[,,] SetRange(float[,,] array, float[,] range, int channel, float rangeLow, float rangeHigh, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			int channelCount = array.GetLength(2);
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					if (range[i, j] >= rangeLow && range[i, j] <= rangeHigh)
					{
						for (int k = 0; k < channelCount; k++)
						{
							array[i, j, k] = 0f;
						}
						array[i, j, channel] = 1f;
					}
				}
			});
			return array;
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000B555C File Offset: 0x000B375C
		public static float[,,] SetRangeBlend(float[,,] array, float[,] range, int channel, float rangeLow, float rangeHigh, float rangeBlendLow, float rangeBlendHigh, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			int channelLength = array.GetLength(2);
			int i;
			Action<int> <>9__0;
			int i2;
			for (i = dmns.x0; i < dmns.x1; i = i2 + 1)
			{
				int z = dmns.z0;
				int z2 = dmns.z1;
				Action<int> body;
				if ((body = <>9__0) == null)
				{
					body = (<>9__0 = delegate(int j)
					{
						float[] array2 = new float[channelLength];
						if (range[i, j] >= rangeLow && range[i, j] <= rangeHigh)
						{
							for (int k = 0; k < channelLength; k++)
							{
								array[i, j, k] = 0f;
							}
							array[i, j, channel] = 1f;
							return;
						}
						if (range[i, j] >= rangeBlendLow && range[i, j] < rangeLow)
						{
							float num = range[i, j] - rangeBlendLow;
							float num2 = rangeLow - rangeBlendLow;
							float num3 = num / num2;
							for (int l = 0; l < channelLength; l++)
							{
								if (l == channel)
								{
									array[i, j, channel] = num3;
								}
								else
								{
									array[i, j, l] *= Mathf.Clamp01(1f - num3);
								}
								array2[l] = array[i, j, l];
							}
							float num4 = array2.Sum();
							for (int m = 0; m < channelLength; m++)
							{
								array2[m] /= num4;
								array[i, j, m] = array2[m];
							}
							return;
						}
						if (range[i, j] > rangeHigh && range[i, j] <= rangeBlendHigh)
						{
							float num5 = range[i, j] - rangeHigh;
							float num6 = rangeBlendHigh - rangeHigh;
							float num7 = num5 / num6;
							float num8 = 1f - num7;
							for (int n = 0; n < channelLength; n++)
							{
								if (n == channel)
								{
									array[i, j, channel] = num8;
								}
								else
								{
									array[i, j, n] *= Mathf.Clamp01(1f - num8);
								}
								array2[n] = array[i, j, n];
							}
							float num9 = array2.Sum();
							for (int num10 = 0; num10 < channelLength; num10++)
							{
								array2[num10] /= num9;
								array[i, j, num10] = array2[num10];
							}
						}
					});
				}
				Parallel.For(z, z2, body);
				i2 = i;
			}
			return array;
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000B5628 File Offset: 0x000B3828
		public static bool[,] SetRange(bool[,] array, float[,] range, bool value, float rangeLow, float rangeHigh, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = new AreaManager.Area(AreaManager.ActiveArea.x0, AreaManager.ActiveArea.x1 * 2, AreaManager.ActiveArea.z0, AreaManager.ActiveArea.z1 * 2);
			}
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					if (range[i * 2, j * 2] > rangeLow && range[i * 2, j * 2] < rangeHigh)
					{
						array[i, j] = value;
					}
				}
			});
			return array;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000B56D4 File Offset: 0x000B38D4
		public static float[,,] SetRiver(float[,,] array, float[,] landHeights, float[,] waterHeights, bool aboveTerrain, int channel, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			int channelLength = array.GetLength(2);
			if (aboveTerrain)
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						if (waterHeights[i, j] > 500f && waterHeights[i, j] > landHeights[i, j])
						{
							for (int k = 0; k < channelLength; k++)
							{
								array[i, j, k] = 0f;
							}
							array[i, j, channel] = 1f;
						}
					}
				});
			}
			else
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						if (waterHeights[i, j] > 500f)
						{
							for (int k = 0; k < channelLength; k++)
							{
								array[i, j, k] = 0f;
							}
							array[i, j, channel] = 1f;
						}
					}
				});
			}
			return array;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000B578C File Offset: 0x000B398C
		public static bool[,] SetRiver(bool[,] array, float[,] landHeights, float[,] waterHeights, bool aboveTerrain, bool value, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = new AreaManager.Area(AreaManager.ActiveArea.x0, AreaManager.ActiveArea.x1 * 2, AreaManager.ActiveArea.z0, AreaManager.ActiveArea.z1 * 2);
			}
			if (aboveTerrain)
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						if (waterHeights[i, j] > 500f && waterHeights[i, j] > landHeights[i, j])
						{
							array[i, j] = value;
						}
					}
				});
			}
			else
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						if (waterHeights[i, j] > 500f)
						{
							array[i, j] = value;
						}
					}
				});
			}
			return array;
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000B5860 File Offset: 0x000B3A60
		public static bool[,] CheckConditions(float[,] array, bool[,] conditionsMet, float minValue, float maxValue)
		{
			int arrayLength = array.GetLength(0);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					if (array[i, j] < minValue)
					{
						conditionsMet[i, j] = true;
					}
					else if (array[i, j] > maxValue)
					{
						conditionsMet[i, j] = true;
					}
				}
			});
			return conditionsMet;
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000B58C0 File Offset: 0x000B3AC0
		public static bool[,] CheckConditions(float[,,] array, bool[,] conditionsMet, int layer, float weight)
		{
			int arrayLength = array.GetLength(0);
			array.GetLength(2);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					if (array[i, j, layer] < weight)
					{
						conditionsMet[i, j] = true;
					}
				}
			});
			return conditionsMet;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000B5930 File Offset: 0x000B3B30
		public static bool[,] CheckConditions(bool[,] array, bool[,] conditionsMet, bool value)
		{
			int arrayLength = array.GetLength(0);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					if (array[i, j] != value)
					{
						conditionsMet[i, j] = true;
					}
				}
			});
			return conditionsMet;
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000B598C File Offset: 0x000B3B8C
		public static float[,] ClampValues(float[,] array, float minValue, float maxValue, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					array[i, j] = Mathf.Clamp(array[i, j], minValue, maxValue);
				}
			});
			return array;
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000B59FC File Offset: 0x000B3BFC
		public static float[,] Rotate(float[,] array, bool CW, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			float[,] newArray = new float[array.GetLength(0), array.GetLength(1)];
			if (CW)
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						newArray[i, j] = array[j, dmns.x1 - i - 1];
					}
				});
			}
			else
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						newArray[i, j] = array[dmns.z1 - j - 1, i];
					}
				});
			}
			return newArray;
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
		public static float[,,] Rotate(float[,,] array, bool CW, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			int channelLength = array.GetLength(2);
			float[,,] newArray = new float[array.GetLength(0), array.GetLength(1), array.GetLength(2)];
			if (CW)
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						for (int k = 0; k < channelLength; k++)
						{
							newArray[i, j, k] = array[j, dmns.x1 - i - 1, k];
						}
					}
				});
			}
			else
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						for (int k = 0; k < channelLength; k++)
						{
							newArray[i, j, k] = array[dmns.z1 - j - 1, i, k];
						}
					}
				});
			}
			return newArray;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000B5B80 File Offset: 0x000B3D80
		public static bool[,] Rotate(bool[,] array, bool CW, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = new AreaManager.Area(AreaManager.ActiveArea.x0, AreaManager.ActiveArea.x1 * 2, AreaManager.ActiveArea.z0, AreaManager.ActiveArea.z1 * 2);
			}
			bool[,] newArray = new bool[array.GetLength(0), array.GetLength(1)];
			if (CW)
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						newArray[i, j] = array[j, dmns.x1 - i - 1];
					}
				});
			}
			else
			{
				Parallel.For(dmns.x0, dmns.x1, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						newArray[i, j] = array[dmns.z1 - j - 1, i];
					}
				});
			}
			return newArray;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000B5C60 File Offset: 0x000B3E60
		public static float[,] Invert(float[,] array, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					array[i, j] = 1f - array[i, j];
				}
			});
			return array;
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x000B5CC4 File Offset: 0x000B3EC4
		public static float[,,] Invert(float[,,] array, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			int channelLength = array.GetLength(2);
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					for (int k = 0; k < channelLength; k++)
					{
						array[i, j, k] = 1f - array[i, j, k];
					}
				}
			});
			return array;
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000B5D38 File Offset: 0x000B3F38
		public static bool[,] Invert(bool[,] array, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = new AreaManager.Area(AreaManager.ActiveArea.x0, AreaManager.ActiveArea.x1 * 2, AreaManager.ActiveArea.z0, AreaManager.ActiveArea.z1 * 2);
			}
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					array[i, j] = !array[i, j];
				}
			});
			return array;
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000B5DC8 File Offset: 0x000B3FC8
		public static float[,] Normalise(float[,] array, float normaliseLow, float normaliseHigh, AreaManager.Area dmns = null)
		{
			if (dmns == null)
			{
				dmns = AreaManager.ActiveArea;
			}
			float highestPoint = 0f;
			float lowestPoint = 1f;
			float heightRange = 0f;
			float normalisedHeightRange = 0f;
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					if (array[i, j] < lowestPoint)
					{
						lowestPoint = array[i, j];
					}
					else if (array[i, j] > highestPoint)
					{
						highestPoint = array[i, j];
					}
				}
			});
			heightRange = highestPoint - lowestPoint;
			normalisedHeightRange = normaliseHigh - normaliseLow;
			Parallel.For(dmns.x0, dmns.x1, delegate(int i)
			{
				for (int j = dmns.z0; j < dmns.z1; j++)
				{
					array[i, j] = normaliseLow + (array[i, j] - lowestPoint) / heightRange * normalisedHeightRange;
				}
			});
			return array;
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000B5EA8 File Offset: 0x000B40A8
		public static float[,] Offset(float[,] array, float offset, bool clampOffset, AreaManager.Area dmns = null)
		{
			float[,] tempArray = array;
			CancellationTokenSource source = new CancellationTokenSource();
			ParallelOptions parallelOptions = new ParallelOptions
			{
				CancellationToken = source.Token
			};
			try
			{
				if (dmns == null)
				{
					dmns = AreaManager.ActiveArea;
				}
				Parallel.For(dmns.x0, dmns.x1, parallelOptions, delegate(int i)
				{
					for (int j = dmns.z0; j < dmns.z1; j++)
					{
						if (clampOffset)
						{
							if (array[i, j] + offset > 1f || array[i, j] + offset < 0f)
							{
								source.Cancel();
							}
							else
							{
								tempArray[i, j] += offset;
							}
						}
						else
						{
							tempArray[i, j] += offset;
						}
					}
				});
			}
			catch (OperationCanceledException)
			{
				return array;
			}
			return tempArray;
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000B5F68 File Offset: 0x000B4168
		public static float[,] HeightToSplat(float[,] array)
		{
			int arrayLength = TerrainManager.SplatMapRes;
			float ratio = (1f * (float)array.GetLength(0) - 1f) / ((float)arrayLength * 1f);
			Debug.Log(ratio.ToString() + " " + arrayLength.ToString());
			float[,] arrayOut = new float[arrayLength, arrayLength];
			if (array == null)
			{
				Debug.Log("null array received");
				return arrayOut;
			}
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					int num = (int)((float)i * ratio);
					int num2 = (int)((float)j * ratio);
					if (num < array.GetLength(0) && num2 < array.GetLength(1))
					{
						arrayOut[i, j] = Mathf.Abs(array[num, num2]);
					}
				}
			});
			return arrayOut;
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000B6030 File Offset: 0x000B4230
		public static float[,,] HeightToSplat(float[,] array, bool[,] spawn, int arrayLength)
		{
			int num = (array.GetLength(0) - 1) / arrayLength;
			float[,,] arrayOut = new float[arrayLength, arrayLength, 3];
			int ratioX = (array.GetLength(0) - 1) / (arrayLength - 1);
			int ratioY = (array.GetLength(1) - 1) / (arrayLength - 1);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					int num2 = i * ratioX;
					int num3 = j * ratioY;
					if (num2 < array.GetLength(0) && num3 < array.GetLength(1))
					{
						if (!spawn[num2, num3])
						{
							arrayOut[i, j, 0] = 1f - array[num2, num3];
							arrayOut[i, j, 1] = array[num2, num3];
							arrayOut[i, j, 2] = 0f;
						}
						else
						{
							arrayOut[i, j, 0] = 0f;
							arrayOut[i, j, 1] = 0f;
							arrayOut[i, j, 2] = 20f;
						}
					}
				}
			});
			return arrayOut;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000B60E0 File Offset: 0x000B42E0
		public static float[,] ShortMapToFloatArray(TerrainMap<short> terrainMap)
		{
			float[,] array = new float[terrainMap.res, terrainMap.res];
			int arrayLength = array.GetLength(0);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					array[i, j] = BitUtility.Short2Float(terrainMap[i, j]);
				}
			});
			return array;
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000B614C File Offset: 0x000B434C
		public static byte[] FloatArrayToByteArray(float[,] array)
		{
			short[] shortArray = new short[array.GetLength(0) * array.GetLength(1)];
			int arrayLength = array.GetLength(0);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				for (int j = 0; j < arrayLength; j++)
				{
					shortArray[i * arrayLength + j] = BitUtility.Float2Short(array[i, j]);
				}
			});
			byte[] array2 = new byte[shortArray.Length * 2];
			Buffer.BlockCopy(shortArray, 0, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000B61D8 File Offset: 0x000B43D8
		public static float[,,] NormaliseMulti(float[,,] array, int texturesAmount)
		{
			Math.Sqrt((double)(array.Length / texturesAmount));
			int arrayLength = array.GetLength(0);
			int channelLength = array.GetLength(2);
			Parallel.For(0, arrayLength, delegate(int i)
			{
				float[] array2 = new float[channelLength];
				for (int j = 0; j < arrayLength; j++)
				{
					for (int k = 0; k < channelLength; k++)
					{
						array2[k] = array[i, j, k];
					}
					float num = array2.Sum();
					for (int l = 0; l < channelLength; l++)
					{
						array2[l] /= num;
						array[i, j, l] = array2[l];
					}
				}
			});
			return array;
		}
	}
}
