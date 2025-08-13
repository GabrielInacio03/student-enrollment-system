<template>
  <div class="container py-4">
    <div class="d-flex flex-wrap justify-content-between align-items-center mb-3">
      <div class="d-flex align-items-center gap-3 mb-4">
        <img src="@/assets/images/logtipo.png" alt="Logo" style="max-height: 60px;" />
        <h2 class="mb-0">📚 Listagem de Alunos</h2>
      </div>
      
      <div class="d-flex flex-wrap gap-2">
        <input
          v-model="search"
          class="form-control"
          placeholder="🔍 Digite sua busca"
        />
        <button class="btn btn-outline-secondary" @click="search = ''">❌ Limpar</button>
        <button class="btn btn-primary" @click="openForm()">➕ Cadastrar Aluno</button>
      </div>
    </div>

    <table class="table table-bordered table-hover">
      <thead class="table-primary">
        <tr>
          <th>RA</th>
          <th>Nome</th>
          <th>CPF</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="isLoading">
          <td colspan="4" class="text-center">🔄 Carregando alunos...</td>
        </tr>
        <tr v-for="student in students" :key="student.id">
          <td>{{ student.ra }}</td>
          <td>{{ student.name }}</td>
          <td>{{ student.cpf }}</td>
          <td>
            <button class="btn btn-warning btn-sm me-1" @click="openForm(student)">
              <i class="bi bi-pencil"></i> Editar
            </button>
            <button class="btn btn-danger btn-sm" @click="confirmDelete(student.id)">
              <i class="bi bi-trash"></i> Excluir
            </button>
          </td>
        </tr>
        <tr v-if="!isLoading && students.length === 0">
          <td colspan="4" class="text-center">Nenhum aluno encontrado.</td>
        </tr>
      </tbody>
    </table>

    <div class="d-flex justify-content-center align-items-center gap-3 mt-3" v-if="totalPages >= 1">
      <button
        class="btn btn-outline-primary"
        @click="refreshList(currentPage - 1)"
        :disabled="currentPage === 1"
      >
        <i class="bi bi-chevron-left"></i> Anterior
      </button>

      <span class="fw-medium">Página {{ currentPage }} de {{ totalPages }}</span>

      <button
        class="btn btn-outline-primary"
        @click="refreshList(currentPage + 1)"
        :disabled="currentPage === totalPages"
      >
        Próxima <i class="bi bi-chevron-right"></i>
      </button>
    </div>

    <StudentForm
      v-if="showForm"
      :student="selectedStudent"
      @close="closeForm"
      @saved="refreshList"
    />
    <ConfirmDeleteModal
      v-if="showDeleteModal"
      message="Tem certeza que deseja excluir este aluno?"
      @confirm="deleteStudent"
      @cancel="showDeleteModal = false"
    />
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, watch } from 'vue'
import StudentForm from '../components/StudentForm.vue'
import ConfirmDeleteModal from '../components/ConfirmDeleteModal.vue'
import { getStudentsPaged, deleteStudentById } from '../services/studentService'
import type { Student } from '../types/Student'

const students = ref<Student[]>([])
const search = ref('')
const showForm = ref(false)
const selectedStudent = ref<Student | null>(null)
const showDeleteModal = ref(false)
const studentToDelete = ref<number | null>(null)
const currentPage = ref(1)
const pageSize = 10
const totalPages = ref(1)
const isLoading = ref(false)

function openForm(student: Student | null = null) {
  selectedStudent.value = student
  showForm.value = true
}

function closeForm() {
  selectedStudent.value = null
  showForm.value = false
}

function confirmDelete(id: number) {
  studentToDelete.value = id
  showDeleteModal.value = true
}

async function deleteStudent() {
  if (studentToDelete.value !== null) {
    try {
      await deleteStudentById(studentToDelete.value)
      refreshList(currentPage.value)
    } catch (error) {
      console.error('Erro ao excluir aluno:', error)
    } finally {
      studentToDelete.value = null
      showDeleteModal.value = false
    }
  }
}

async function refreshList(page = 1) {
  isLoading.value = true
  try {
    const data = await getStudentsPaged(page, pageSize, search.value)
    students.value = Array.isArray(data.items) ? data.items : []
    totalPages.value = data.totalPages ?? 1
    currentPage.value = data.currentPage ?? 1
  } catch (error) {
    console.error('Erro ao buscar alunos:', error)
    students.value = []
  } finally {
    isLoading.value = false
  }
}

let searchTimeout: ReturnType<typeof setTimeout>
watch(search, () => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    refreshList(1)
  }, 500)
})

onMounted(() => refreshList())
</script>

<style scoped>
</style>