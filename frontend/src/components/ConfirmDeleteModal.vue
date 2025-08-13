<template>
  <div class="modal-overlay">
    <div class="modal-content bg-white p-4 rounded shadow text-center">
      <p class="mb-3">{{ message }}</p>
      <div class="d-flex justify-content-center gap-3">
        <button class="btn btn-danger" @click="$emit('confirm')">
          <i class="bi bi-trash"></i> Sim, excluir
        </button>
        <button class="btn btn-secondary" @click="$emit('cancel')">
          <i class="bi bi-x-circle"></i> Cancelar
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{ message: string }>()
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0; left: 0;
  width: 100%; height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex; justify-content: center; align-items: center;
  z-index: 1000;
}

.modal-content {
  width: 100%;
  max-width: 400px;
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from { transform: scale(0.95); opacity: 0; }
  to { transform: scale(1); opacity: 1; }
}
</style>