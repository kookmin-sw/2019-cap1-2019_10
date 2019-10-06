from rest_framework.response import Response


def ok(request, data=''):
    return Response(data)


def created(request, data=''):
    return Response(data)


def noContent(request, data=''):
    return Response(data)

