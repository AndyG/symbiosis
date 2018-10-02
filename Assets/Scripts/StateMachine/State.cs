using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State<T>
{
  void OnStateEnter(T context);
  void OnStateExit(T context);
  State<T> Tick(T context);
}
