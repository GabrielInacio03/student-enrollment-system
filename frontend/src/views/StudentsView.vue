<template>

  <div class="student-list">
    <header class="header">
      <h2>📚 Listagem de Alunos</h2>
      <div class="actions">
        <input v-model="search" placeholder="🔍 Buscar por nome, RA ou CPF..." />
        <button @click="openForm()">➕ Cadastrar Aluno</button>
      </div>
    </header>

    <table class="student-table">
      <thead>
        <tr>
          <th>RA</th>
          <th>Nome</th>
          <th>CPF</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="student in students" :key="student.id">
          <td>{{ student.ra }}</td>
          <td>{{ student.name }}</td>
          <td>{{ student.cpf }}</td>
          <td>
            <button @click="openForm(student)">✏️ Editar</button>
            <button @click="confirmDelete(student.id)">Excluir</button>
          </td>
        </tr>
        <tr v-if="students.length === 0">
          <td colspan="4">Nenhum aluno encontrado.</td>
        </tr>
      </tbody>
    </table>

    <div class="pagination" v-if="totalPages >= 1">
      <button
        @click="refreshList(currentPage - 1)"
        :disabled="currentPage === 1"
        class="page-btn"
      >
        <i class="bi bi-chevron-left"></i> Anterior
      </button>

      <span class="page-info">
        Página {{ currentPage }} de {{ totalPages }}
      </span>

      <button
        @click="refreshList(currentPage + 1)"
        :disabled="currentPage === totalPages"
        class="page-btn"
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
.student-list {
  padding: 1rem;
  max-width: 800px;
  margin: auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.actions {
  display: flex;
  gap: 0.5rem;
}

.student-table {
  width: 100%;
  border-collapse: collapse;
}

.student-table th,
.student-table td {
  border: 1px solid #ccc;
  padding: 0.5rem;
  text-align: left;
}

.student-table th {
  background-color: #f0f0f0;
}

button {
  margin-right: 0.5rem;
  cursor: pointer;
}

.pagination {
  margin-top: 1rem;
  display: flex;
  justify-content: center;
  gap: 1rem;
}
.pagination {
  margin-top: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
}

.page-btn {
  display: flex;
  align-items: center;
  gap: 0.3rem;
  padding: 0.4rem 0.8rem;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  transition: background-color 0.2s ease;
}

.page-btn:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.page-btn:hover:not(:disabled) {
  background-color: #0056b3;
}

.page-info {
  font-weight: 500;
}
</style>