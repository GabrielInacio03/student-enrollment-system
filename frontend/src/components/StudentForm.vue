<template>
  <div class="form-overlay">
    <div class="form-container bg-white p-4 rounded shadow">
      <h4 class="mb-3">
        {{ student?.id ? '✏️ Editar Aluno' : '➕ Cadastrar Aluno' }}
      </h4>

      <form @submit.prevent="save">
        <div class="mb-3">
          <label for="name" class="form-label">Nome:</label>
          <input id="name" v-model="form.name" required class="form-control" />
        </div>

        <div class="mb-3">
          <label for="email" class="form-label">Email:</label>
          <input id="email" v-model="form.email" required type="email" class="form-control" />
        </div>

        <div class="mb-3">
          <label for="ra" class="form-label">RA:</label>
          <input id="ra" v-model="form.ra" required class="form-control" />
        </div>

        <div class="mb-3">
          <label for="cpf" class="form-label">CPF:</label>
          <input id="cpf" v-model="form.cpf" required class="form-control" />
        </div>

        <div class="d-flex justify-content-between mt-4">
          <button type="submit" class="btn btn-success">
            <i class="bi bi-save"></i> Salvar
          </button>
          <button type="button" class="btn btn-danger" @click="$emit('close')">
            <i class="bi bi-x-circle"></i> Cancelar
          </button>
        </div>
      </form>

      <transition name="fade">
        <div v-if="errorMessages.length" class="alert alert-danger mt-4">
          <ul class="mb-0">
            <li v-for="(msg, index) in errorMessages" :key="index">{{ msg }}</li>
          </ul>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import type { Student } from '../types/Student'
import { createStudent, updateStudent } from '../services/studentService'

const props = defineProps<{ student: Student | null }>()
const emit = defineEmits(['close', 'saved'])
const errorMessages = ref<string[]>([])

const form = ref<Student>({
  id: 0,
  name: '',
  email: '',
  ra: '',
  cpf: ''
})

watch(() => props.student, (newVal) => {
  form.value = newVal
    ? { ...newVal }
    : { id: 0, name: '', email: '', ra: '', cpf: '' }
}, { immediate: true })

async function save() {
  errorMessages.value = []
  try {
    if (form.value.id) {
      await updateStudent(form.value.id, form.value)
    } else {
      await createStudent(form.value)
    }
    emit('saved')
    emit('close')
  } catch (error: any) {
    const response = error.response?.data
    errorMessages.value = []

    if (response?.errors && typeof response.errors === 'object') {
      const validationErrors = Object.values(response.errors).flat()
      errorMessages.value = validationErrors as string[]
    } else if (Array.isArray(response?.erros)) {
      errorMessages.value = response.erros
    } else {
      errorMessages.value = ['Erro inesperado ao salvar aluno.']
    }
  }
}
</script>

<style scoped>
.form-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.form-container {
  width: 100%;
  max-width: 400px;
  animation: slideIn 0.3s ease;
}

@keyframes slideIn {
  from { transform: translateY(-20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>