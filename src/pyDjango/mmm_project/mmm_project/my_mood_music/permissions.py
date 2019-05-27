from rest_framework import permissions

class IsOwnerOnly(permissions.BasePermission):
    def has_object_permission(selfself, request, view, obj):
        return obj.subscriber == request.user or request.user.is_superuser


class IsOwnerOrReadOnly(permissions.BasePermission):

    def has_object_permission(self, request, view, obj):
        if request.method in permissions.SAFE_METHODS:
            return True

        return obj.subscriber == request.user or request.user.is_superuser


# 소유자나 관리자인 경우에만 사용 가능하도록 하는 권한
# 소유자나 관리자가 아닌 경우 GET이나 HEAD 같은 조회 기능만 가능하도록 하는 권한