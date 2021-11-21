# ActivityMapper

## Problem statement

Ability to define group of Activities (defined by user defined Enum):
- each Activity has it's own input and output type
- utilities to run matching activities by input type
- utilities to dynamicaly map other data models into activity input types
- utilities to allow triggering Activity by Id (from Enum that defines Activity group) and JSON input

This repository contains ActivityMapper library that is PoC implementation of those requirements.

## Implementation details

Solution items:
- _ActivityMapper_ - library implementation 
- _ActivityMapper.Autofac_ - dependency injection module and helpers for dynamic _IActivity_ /  _IActivityInputMapper_ registration
- _ActivityMapper.Tests_ - unit tests (with 97.9% code coverage)
- _ActivityMapper.Demo_ - demo project with:
    - sample activities and mapper
    - activity invocation by direct input
    - activity invocation by mapped input
    - activity invocation by JSON input

Output:
```
================ DemoDirectExecutionAsync ================
[
  {
    "ActivityId": 0,
    "ActivityInput": {
      "Name": "test"
    },
    "ActivityOutput": {
      "Result": "test (in SampleA)"
    }
  },
  {
    "ActivityId": 1,
    "ActivityInput": {
      "Name": "test"
    },
    "ActivityOutput": {
      "Result": "test (in SampleB)"
    }
  }
]

============= DemoDirectExecutionAsync (auto) ============
[
  {
    "ActivityId": 0,
    "ActivityInput": {
      "Name": "Foo"
    },
    "ActivityOutput": {
      "Result": "Foo (in SampleA)"
    }
  },
  {
    "ActivityId": 1,
    "ActivityInput": {
      "Name": "Foo"
    },
    "ActivityOutput": {
      "Result": "Foo (in SampleB)"
    }
  }
]

========= DemoDirectExecutionAsync (target type) =========
[
  {
    "ActivityId": 0,
    "ActivityInput": {
      "Name": "Foo"
    },
    "ActivityOutput": {
      "Result": "Foo (in SampleA)"
    }
  },
  {
    "ActivityId": 1,
    "ActivityInput": {
      "Name": "Foo"
    },
    "ActivityOutput": {
      "Result": "Foo (in SampleB)"
    }
  }
]

================= DemoJsonExecutionAsync =================
[
  {
    "ActivityId": 0,
    "ActivityInput": {
      "Name": "Bar"
    },
    "ActivityOutput": {
      "Result": "Bar (in SampleA)"
    }
  },
  {
    "ActivityId": 1,
    "ActivityInput": {
      "Name": "Bar"
    },
    "ActivityOutput": {
      "Result": "Bar (in SampleB)"
    }
  }
]
```

## Potential improvements

There are a couple of ideas for further exploration:
- better Demo project
- automatic execution of Activity chains
- alternatives for _InOutTypes_ and _ActivityBase_ / _ActivityInputMapperBase_ classes
- better Autofac lifetime management
- AutoMapper integration
- defining result type for Activity group
